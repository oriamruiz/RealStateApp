using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Helpers.Sessions;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Users;
using RealStateApp.WebApi.Controllers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RestaurantApp.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [SwaggerTag("Manejo de cuentas")]
    public class AccountController : BaseApiController
    {

        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }


        
        [HttpPost("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Login",
            Description = "Logea un usuario mediante nombre de usuario y contrasenia"
            )]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                AuthenticationResponse response = await _userService.LoginByApiAsync(request);

                if (response != null && response.HasError != true)
                {
                    HttpContext.Session.Set<AuthenticationResponse>("user", response);
                    return Ok(response);
                }
                else
                {
                    return Ok(response.Error);
                }

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);


            }
            

        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("RegisterAdmin")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponse))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Creacion de usuario admin",
            Description = "Recibe los parametros necesarios para crear un usuario admin"
            )]
        public async Task<IActionResult> RegisterAdmin([FromBody] CreateAdminOrDeveloperViewModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                request.Role = Roles.ADMIN.ToString();
                RegisterResponse response = await _userService.RegisterAdminOrDeveloperAsync(request);
                
                if (response != null && response.HasError != true)
                {
                    return StatusCode(StatusCodes.Status201Created);
                }
                else
                {
                    return Ok(response.Error);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);


            }
           
        }



        [Authorize(Roles = "ADMIN")]
        [HttpPost("RegisterDeveloper")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponse))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Creacion de usuario desarrollador",
            Description = "Recibe los parametros necesarios para crear un usuario desarrollador"
            )]
        public async Task<IActionResult> RegisterDeveloper([FromBody] CreateAdminOrDeveloperViewModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                request.Role = Roles.DESARROLLADOR.ToString();
                RegisterResponse response = await _userService.RegisterAdminOrDeveloperAsync(request);

                if (response != null && response.HasError != true)
                {
                    return StatusCode(StatusCodes.Status201Created);
                }
                else
                {
                    return Ok(response.Error);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);


            }
            

        }
    }
}
