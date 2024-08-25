using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Security.Cryptography;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Infrastructure.Identity.Entities;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Enums;
using Microsoft.AspNetCore.WebUtilities;
using RealStateApp.Core.Application.Dtos.Email;
using RealStateApp.Core.Application.ViewModels.Agents;
using RealStateApp.Core.Application.Dtos.Agent;
using Microsoft.EntityFrameworkCore;
using InternetBankingApp.Core.Application.ViewModels.Users;
using RealStateApp.Core.Application.Dtos.Admin;
using RealStateApp.Core.Application.Dtos.Developer;
using Microsoft.IdentityModel.Tokens;
using RealStateApp.Core.Domain.Settings;
using System.Security.Claims;

namespace RealStateApp.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly JWTSettings _jWTSettings;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper, IEmailService emailService, IOptions<JWTSettings> jWTSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _emailService = emailService;
            _jWTSettings = jWTSettings.Value;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByNameAsync(request.UserName);


            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No se encontro una cuenta con este nombre de usuario: {response.UserName}";
                return response;
            }

            var roleslist = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            if (roleslist.Contains(Roles.DESARROLLADOR.ToString()))
            {
                response.HasError = true;
                response.Error = "Siendo desarrollador no tiene permiso para usar la web app";
                return response;
            }

            if (!user.EmailConfirmed)
            {
                response.HasError = true;

                if (roleslist.Contains(Roles.CLIENTE.ToString()))
                {
                    response.Error = $"la cuenta de usuario: {response.UserName} no ha realizado la confirmacion de correo {user.Email}";
                }
                else
                {
                    response.Error = $"la cuenta de usuario: {response.UserName} no esta activa, contactese con un administrador";
                }


                return response;

            }

            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, lockoutOnFailure: false);


            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Creedenciales incorrectas para: {request.UserName}";
                return response;
            }


            response.Id = user.Id;
            response.UserName = user.UserName;
            response.Email = user.Email;
            response.Roles = roleslist.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.HasError = false;

            return response;

        }

        public async Task<AuthenticationResponse> AuthenticateByApiAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No se encontro una cuenta con este nombre de usuario: {response.UserName}";
                return response;
            }

            var roleslist = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            if(roleslist.Contains(Roles.CLIENTE.ToString()) || roleslist.Contains(Roles.AGENTE.ToString()))
            {
                response.HasError = true;
                response.Error = $"Si usted es agente o cliente no tiene permiso a usar la web api.";
                return response;

            }

            if (!user.IsActive)
            {
                response.HasError = true;
                response.Error = $"Este usuario esta inactivo";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Creedenciales incorrectas para: {request.UserName}";
                return response;
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

            response.Id = user.Id;
            response.UserName = user.UserName;
            response.Email = user.Email;

            response.IsVerified = user.IsActive;
            response.Roles = roleslist.ToList();
            response.HasError = false;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshtoken = GenerateRefreshToken();
            response.RefreshToken = refreshtoken.Token;

            return response;

        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }


        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request, string origin ="")
        {
            RegisterResponse response = new();
            response.HasError = false;

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);

            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"Ya existe una cuenta con este nombre de usuario: {request.UserName}. ";
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error += $"Ya existe una cuenta con este correo: {request.Email}. ";
            }

            if(response.HasError)
            {
                return response;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                

            };


            if (request.Role.Contains(Roles.ADMIN.ToString()) || request.Role.Contains(Roles.DESARROLLADOR.ToString()))
            {
                user.Dni = request.Dni;
                user.EmailConfirmed = true;
                user.IsActive = true;
            }
            else
            {
                user.IsActive = false;
                user.EmailConfirmed = false;
                user.PhoneNumber = request.Phone;
            }

            var result = await _userManager.CreateAsync(user, request.Password);


            if (result.Succeeded)
            {
                response.Id = user.Id;

                await _userManager.AddToRoleAsync(user, request.Role);

                if (request.Role == Roles.CLIENTE.ToString())
                {
                    var verificationuri = await SendVerificationEmailUrl(user, origin);
                    await _emailService.SendAsync(new EmailRequest()
                    {
                        To = user.Email,
                        Body = $"Favor confirmar tu cuenta a traves de esta url: {verificationuri}",
                        Subject = "Confirmar cuenta"

                    });

                }

            }
            else
            {
                response.HasError = true;
                response.Error += $"Ocurrio un error creando la cuenta, intentelo de nuevo.";
                return response;
            }

            return response;

        }


        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return "No existe una cuenta con este usuario.";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
                return $"Cuenta confirmada con el correo {user.Email}.";
            }
            else
            {
                return "Ocurrio un error confirmando esta cuenta.";
            }


        }

        public async Task UpdateImgAsync(UpdateImageRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

            if (user != null)
            {
                user.Id = request.Id;
                user.AccountImgUrl = request.AccountImgUrl;

                await _userManager.UpdateAsync(user);
            }

        }


        public async Task<List<GetActiveAgentResponse>> GetAllActiveAgentsAsync()
        {
            var agents = await _userManager.GetUsersInRoleAsync(Roles.AGENTE.ToString());
            
            var activeagents =  agents.Where(u=> u.EmailConfirmed == true).ToList();

            var activeAgentResponses = activeagents.Select(agent => new GetActiveAgentResponse
            {
                Id = agent.Id,
                FirstName = agent.FirstName,
                LastName = agent.LastName,
                FullName = $"{agent.FirstName} {agent.LastName}",
                AccountImgUrl = agent.AccountImgUrl,
            
            }).ToList();

            return activeAgentResponses;

        }

        public async Task<UpdateAgentResponse> GetByIdAgentUpdateViewModelAsync(string Id)
        {
            UpdateAgentResponse response = new();

            var agents = await _userManager.GetUsersInRoleAsync(Roles.AGENTE.ToString());

            var agent = agents.FirstOrDefault(u=> u.Id == Id);

            if (agent != null)
            {
                response.Id = agent.Id;
                response.FirstName = agent.FirstName;
                response.LastName = agent.LastName;
                response.Phone = agent.PhoneNumber;
                var roleslist = await _userManager.GetRolesAsync(agent).ConfigureAwait(false);
                response.Roles = roleslist.ToList();
                response.AccountImgUrl = agent.AccountImgUrl;
            }
            else
            {
                response.HasError = true;
                response.Error = $"No existe un agente con este id. ";
                return response;
            }


            return response;

        }


        public async Task<UpdateAgentResponse> UpdateAgentAsync(UpdateAgentRequest request)
        {
            UpdateAgentResponse response = new();
            response.HasError = false;

            var users = await _userManager.GetUsersInRoleAsync(Roles.AGENTE.ToString());

            var user = users.FirstOrDefault(u => u.Id == request.Id);

            if (user != null)
            {

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.PhoneNumber = request.Phone;



                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {

                    response.Id = user.Id;
                    response.FirstName = user.FirstName;
                    response.Phone = user.PhoneNumber;

                }
                else
                {

                    response.HasError = true;
                    response.Error = $"Ocurrio un error actualizando la cuenta, intentelo de nuevo.";
                    return response;
                }
            }
            else
            {

                response.HasError = true;
                response.Error = $"No se encontro el usuario a editar";
                return response;
            }

            return response;
        }

        public async Task<UpdateAdminResponse> GetByIdAdminUpdateViewModelAsync(string Id)
        {
            UpdateAdminResponse response = new();

            var admins = await _userManager.GetUsersInRoleAsync(Roles.ADMIN.ToString());

            var admin = admins.FirstOrDefault(u => u.Id == Id);

            if (admin != null)
            {
                response.Id = admin.Id;
                response.FirstName = admin.FirstName;
                response.LastName = admin.LastName;
                response.Dni = admin.Dni;
                response.Email = admin.Email;
                response.UserName = admin.UserName;
            }
            else
            {
                response.HasError = true;
                response.Error = $"No existe un usuario con este id. ";
                return response;
            }


            return response;

        }
        
        public async Task<UpdateAdminResponse> UpdateAdminAsync(UpdateAdminRequest request)
        {
            UpdateAdminResponse response = new();
            response.HasError = false;

            var admins = await _userManager.GetUsersInRoleAsync(Roles.ADMIN.ToString());

            var admin = admins.FirstOrDefault(u => u.Id == request.Id);

            if (admin != null)
            {

                var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);

                if (userWithSameUserName != null && userWithSameUserName.Id != admin.Id)
                {
                    response.HasError = true;
                    response.Error = $"Ya existe una cuenta con este nombre de usuario: {request.UserName}. ";
                }

                var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

                if (userWithSameEmail != null && userWithSameEmail.Id != admin.Id)
                {
                    response.HasError = true;
                    response.Error += $"Ya existe una cuenta con este correo: {request.Email}. ";
                }

                if (response.HasError)
                {
                    return response;
                }

                admin.FirstName = request.FirstName;
                admin.LastName = request.LastName;
                admin.Dni = request.Dni;
                admin.Email = request.Email;
                admin.UserName = request.UserName;

                var result = await _userManager.UpdateAsync(admin);

                await _userManager.RemovePasswordAsync(admin);
                await _userManager.AddPasswordAsync(admin, request.Password);

                if (result.Succeeded)
                {

                    response.Id = admin.Id;
                    response.FirstName = admin.FirstName;
                    response.Dni = admin.Dni;
                    response.Email = admin.Email;
                    response.LastName= admin.LastName;

                }
                else
                {

                    response.HasError = true;
                    response.Error = $"Ocurrio un error actualizando la cuenta, intentelo de nuevo.";
                    return response;
                }
            }
            else
            {

                response.HasError = true;
                response.Error = $"No se encontro el admin a editar";
                return response;
            }

            return response;
        }


        public async Task<UpdateDeveloperResponse> GetByIdDeveloperUpdateViewModelAsync(string Id)
        {
            UpdateDeveloperResponse response = new();

            var developers = await _userManager.GetUsersInRoleAsync(Roles.DESARROLLADOR.ToString());

            var Developer = developers.FirstOrDefault(u => u.Id == Id);

            if (Developer != null)
            {
                response.Id = Developer.Id;
                response.FirstName = Developer.FirstName;
                response.LastName = Developer.LastName;
                response.Dni = Developer.Dni;
                response.Email = Developer.Email;
                response.UserName = Developer.UserName;
            }
            else
            {
                response.HasError = true;
                response.Error = $"No existe un usuario con este id. ";
                return response;
            }


            return response;

        }

        public async Task<UpdateDeveloperResponse> UpdateDeveloperAsync(UpdateDeveloperRequest request)
        {
            UpdateDeveloperResponse response = new();
            response.HasError = false;

            var developers = await _userManager.GetUsersInRoleAsync(Roles.DESARROLLADOR.ToString());

            var developer = developers.FirstOrDefault(u => u.Id == request.Id);

            if (developer != null)
            {

                var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);

                if (userWithSameUserName != null && userWithSameUserName.Id != developer.Id)
                {
                    response.HasError = true;
                    response.Error = $"Ya existe una cuenta con este nombre de usuario: {request.UserName}. ";
                }

                var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

                if (userWithSameEmail != null && userWithSameEmail.Id != developer.Id)
                {
                    response.HasError = true;
                    response.Error += $"Ya existe una cuenta con este correo: {request.Email}. ";
                }

                if (response.HasError)
                {
                    return response;
                }

                developer.FirstName = request.FirstName;
                developer.LastName = request.LastName;
                developer.Dni = request.Dni;
                developer.Email = request.Email;
                developer.UserName = request.UserName;

                var result = await _userManager.UpdateAsync(developer);

                await _userManager.RemovePasswordAsync(developer);
                await _userManager.AddPasswordAsync(developer, request.Password);

                if (result.Succeeded)
                {

                    response.Id = developer.Id;
                    response.FirstName = developer.FirstName;
                    response.Dni = developer.Dni;
                    response.Email = developer.Email;
                    response.LastName = developer.LastName;

                }
                else
                {

                    response.HasError = true;
                    response.Error = $"Ocurrio un error actualizando la cuenta, intentelo de nuevo.";
                    return response;
                }
            }
            else
            {

                response.HasError = true;
                response.Error = $"No se encontro el desarrollador a editar";
                return response;
            }

            return response;
        }



        public async Task<GetUserResponse> GetById(string Id)
        {
            GetUserResponse response = new();

            ApplicationUser user = await _userManager.FindByIdAsync(Id);

            if (user != null)
            {
                response.Id = user.Id;
                response.UserName = user.UserName;
                response.Email = user.Email;
                var roleslist = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                response.Roles = roleslist.ToList();
                response.IsActive = user.IsActive;
                response.EmailConfirmed = user.EmailConfirmed;
                response.AccountImgUrl = user.AccountImgUrl;
                response.Phone = user.PhoneNumber;
                response.LastName = user.LastName;
                response.FirstName = user.FirstName;
                response.Dni = user.Dni;
            }
            else
            {
                response.HasError = true;
                response.Error = $"No existe un usuario con este id. ";
                return response;
            }


            return response;
        }

        public async Task DeleteAgentAsync(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            await _userManager.DeleteAsync(user);
        
        }

        public async Task<List<GetUserResponse>> GetAllByRoleAsync(string role)
        {

            List<GetUserResponse> response = new();
            var users = await _userManager.GetUsersInRoleAsync(role);
            
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                response.Add(new GetUserResponse
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList(),
                    Phone = user.PhoneNumber,
                    IsActive = user.IsActive,
                    EmailConfirmed = user.EmailConfirmed,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    Dni = user.Dni,
                });
            }

            return response;
        }

        public async Task<ChangeStatusResponse> ChangeStatusAsync(ChangeStatusRequest request)
        {
            ChangeStatusResponse response = new();
            response.HasError = false;

            var user = await _userManager.FindByIdAsync(request.Id);

            if (user != null)
            {
                user.IsActive = request.IsActive;
                user.EmailConfirmed = request.EmailConfirmed;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {

                    response.Id = user.Id;
                    response.FirstName = user.FirstName;
                    response.LastName = user.LastName;
                    response.Email = user.Email;

                }
                else
                {

                    response.HasError = true;
                    response.Error = $"Ocurrio un error actualizando la cuenta, intentelo de nuevo.";
                    return response;
                }
            }
            else
            {

                response.HasError = true;
                response.Error = $"No se encontro el usuario a editar";
                return response;
            }

            return response;
        }

        #region private methods

        private async Task<string> SendVerificationEmailUrl(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ConfirmEmail";
            var uri = new Uri(string.Concat($"{origin}/", route));
            var verificationurl = QueryHelpers.AddQueryString(uri.ToString(), "userId", user.Id);
            verificationurl = QueryHelpers.AddQueryString(verificationurl, "token", code);

            return verificationurl;
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                Token = RandomTokenString(),
            };
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            var userclaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleclaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleclaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userclaims)
            .Union(roleclaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTSettings.Key));
            var signinCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jWTSettings.Issuer,
            audience: _jWTSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jWTSettings.DurationInMinutes),
                signingCredentials: signinCredentials
                );

            return jwtSecurityToken;
        }


        #endregion



    }


}
