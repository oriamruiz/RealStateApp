using AutoMapper;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetBankingApp.Core.Application.ViewModels.Users;
using RealStateApp.Core.Application.ViewModels.Users;
using System.Data;
using RealStateApp.Core.Application.ViewModels.Agents;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.ViewModels.Admins;
using RealStateApp.Core.Application.Dtos.Admin;
using RealStateApp.Core.Application.Dtos.Developer;
using RealStateApp.Core.Application.ViewModels.Developers;

namespace RealStateApp.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountService _accountService;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyImprovementRepository _propertyImprovementRepository;
        private readonly IFavoritePropertyRepository _favoritePropertyRepository;
        private readonly IMapper _mapper;


        public UserService(IAccountService accountService, IMapper mapper, IPropertyRepository productRepository, IPropertyImprovementRepository propertyImprovementRepository, IFavoritePropertyRepository favoritePropertyRepository)
        {
            _accountService = accountService;
            _mapper = mapper;
            _propertyRepository = productRepository;
            _propertyImprovementRepository = propertyImprovementRepository;
            _favoritePropertyRepository = favoritePropertyRepository;
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest request = _mapper.Map<AuthenticationRequest>(vm);

            AuthenticationResponse response = await _accountService.AuthenticateAsync(request);

            return response;
        }

        public async Task<AuthenticationResponse> LoginByApiAsync(AuthenticationRequest request)
        {

            AuthenticationResponse response = await _accountService.AuthenticateByApiAsync(request);

            return response;
        }

        public async Task<RegisterResponse> RegisterClientOrAgentAsync(CreateAgentOrClientViewModel vm, string origin)
        {
            RegisterRequest request = _mapper.Map<RegisterRequest>(vm);

            return await _accountService.RegisterAsync(request, origin);

            
        }

        public async Task<RegisterResponse> RegisterAdminOrDeveloperAsync(CreateAdminOrDeveloperViewModel vm)
        {
            RegisterRequest request = _mapper.Map<RegisterRequest>(vm);
            return await _accountService.RegisterAsync(request);
        }

        

        public async Task<UpdateAgentResponse> UpdateAgentAsync(UpdateAgentViewModel vm)
        {
            UpdateAgentRequest request = _mapper.Map<UpdateAgentRequest>(vm);

            var response = await _accountService.UpdateAgentAsync(request);

            return response;

        }

        public async Task<UpdateAdminResponse> UpdateAdminAsync(UpdateAdminViewModel vm)
        {
            UpdateAdminRequest request = _mapper.Map<UpdateAdminRequest>(vm);

            var response = await _accountService.UpdateAdminAsync(request);

            return response;

        }

        public async Task<UpdateDeveloperResponse> UpdateDeveloperAsync(UpdateDeveloperViewModel vm)
        {
            UpdateDeveloperRequest request = _mapper.Map<UpdateDeveloperRequest>(vm);

            var response = await _accountService.UpdateDeveloperAsync(request);

            return response;

        }

        public async Task<DeleteAgentViewModel> DeleteAgentAsync(string Id)
        {

            DeleteAgentViewModel vm = new();
            vm.HasError = false;

            var agents = await _accountService.GetAllByRoleAsync(Roles.AGENTE.ToString());
            var agenttodelete = agents.FirstOrDefault(a => a.Id == Id);


            if (agenttodelete == null)
            {
                vm.HasError = true;
                vm.Error = "El agente que intentas borrar no existe";
            }
            else
            {
                vm.Id = agenttodelete.Id;

                var agentproperties = await _propertyRepository.GetAllByAgentIdAsync(Id);
                if(agentproperties != null && agentproperties.Count>0)
                {
                    var propertiesId = agentproperties.Select(x => x.Id).ToList();

                    vm.Properties = propertiesId;

                    foreach (var propId in propertiesId)
                    {
                        var propertyToDelete = await _propertyRepository.GetByIdAsync(propId);

                        if (propertyToDelete != null)
                        {
                            var improvements = await _propertyImprovementRepository.GetAllAsync();

                            var propertyimprovements = improvements.Where(pi => pi.PropertyId == propertyToDelete.Id).ToList();

                            if (propertyimprovements != null && propertyimprovements.Count > 0)
                            {
                                foreach (var imp in propertyimprovements)
                                {
                                    await _propertyImprovementRepository.DeleteAsync(imp);
                                }
                            }


                            var favoriteProperties = await _favoritePropertyRepository.GetAllAsync();

                            var favoritePropertiesToDelete = favoriteProperties.Where(f => f.PropertyId == propertyToDelete.Id).ToList();

                            if (favoritePropertiesToDelete != null && favoritePropertiesToDelete.Count > 0)
                            {
                                foreach (var fav in favoritePropertiesToDelete)
                                {
                                    await _favoritePropertyRepository.DeleteAsync(fav);
                                }
                            }


                            await _propertyRepository.DeleteAsync(propertyToDelete);

                        }

                    }
                }
                

                await _accountService.DeleteAgentAsync(agenttodelete.Id);
                
            }

            return vm;

            

        }

        public async Task SignOutAsync()
        {
            await _accountService.SignOutAsync();
        }


        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            return await _accountService.ConfirmAccountAsync(userId, token);
        }

        public async Task UpdateImgViewModel(UpdateImgViewModel vm)
        {
            
            UpdateImageRequest updateImageRequest = _mapper.Map<UpdateImageRequest>(vm);

            await _accountService.UpdateImgAsync(updateImageRequest);

        }







        public async Task<UserViewModel> ChangeStatusUser(string id)
        {
            
            UserViewModel response = new();


            ChangeStatusRequest updaterequest = new();
            var user = await _accountService.GetById(id);

            if (!user.HasError)
            {
                response.Id = user.Id;

                user.IsActive = !user.IsActive;
                user.EmailConfirmed = !user.EmailConfirmed;

                updaterequest.IsActive = user.IsActive;
                updaterequest.EmailConfirmed = user.EmailConfirmed;
                updaterequest.Id = user.Id;

                var updateresponse = await _accountService.ChangeStatusAsync(updaterequest);

                if(updateresponse.HasError)
                {
                    response.HasError = user.HasError;
                    response.Error = user.Error;
                    return response;
                }

                response = _mapper.Map<UserViewModel>(updateresponse);

            }
            else
            {
                response.HasError = user.HasError;
                response.Error = user.Error;
                return response;
            }
            
            return response;


        }

        public async Task<UserViewModel> GetByIdAsync(string id)
        {
            var user =await _accountService.GetById(id);

            UserViewModel response = _mapper.Map<UserViewModel>(user);

            return response;
        }

        public async Task<UpdateAgentViewModel> GetByIdUpdateAgentViewModelAsync(string id)
        {
            UpdateAgentViewModel response = null;
            
            var agent = await _accountService.GetByIdAgentUpdateViewModelAsync(id);

            response = _mapper.Map<UpdateAgentViewModel>(agent);

            return response;
        }

        public async Task<UpdateAdminViewModel> GetByIdUpdateAdminViewModelAsync(string id)
        {
            UpdateAdminViewModel response = null;

            var admin = await _accountService.GetByIdAdminUpdateViewModelAsync(id);

            response = _mapper.Map<UpdateAdminViewModel>(admin);

            return response;
        }

        public async Task<UpdateDeveloperViewModel> GetByIdUpdateDeveloperViewModelAsync(string id)
        {
            UpdateDeveloperViewModel response = null;

            var developer = await _accountService.GetByIdDeveloperUpdateViewModelAsync(id);

            response = _mapper.Map<UpdateDeveloperViewModel>(developer);

            return response;
        }



        public async Task<string> RedirectByRoleAsync(List<string> roles)
        {
            string controller = "";

            if (roles.Contains(Roles.ADMIN.ToString()))
            {
                controller = "AdminHome";
            }
            else if (roles.Contains(Roles.DESARROLLADOR.ToString()))
            {
                controller = "DeveloperHome";
            }
            else if (roles.Contains(Roles.CLIENTE.ToString()))
            {
                controller = "ClientHome";
            }
            else if (roles.Contains(Roles.AGENTE.ToString()))
            {
                controller = "AgentHome";
            }

            return controller;
        }

        public async Task<List<GetActiveAgentViewModel>> GetAllActiveAgentsAsync()
        {
            var activeagents = await _accountService.GetAllActiveAgentsAsync();

            
            List<GetActiveAgentViewModel>  activeagentvm = _mapper.Map< List<GetActiveAgentViewModel>>(activeagents);

            activeagentvm = activeagentvm.OrderBy(a=> a.FirstName).ToList();

            return activeagentvm;
        }

        public async Task<List<GetActiveAgentViewModel>> GetAgentsByNameViewModel(string Name)
        {

            List<GetActiveAgentViewModel> agents = await GetAllActiveAgentsAsync();

            if (agents != null || agents.Count > 0 ) 
            {
                if(Name != null)
                {
                    agents = agents.Where(a => a.FullName.Contains(Name, StringComparison.OrdinalIgnoreCase)).ToList();
                }
            }
            
            return agents;
        
        }


        





    }
}
