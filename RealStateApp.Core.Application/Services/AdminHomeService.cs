using AutoMapper;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.AdminHome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Services
{
    public class AdminHomeService : IAdminHomeService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IAccountService _accountservice;
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly ISaleTypeRepository _saletypeRepository;
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;
        public AdminHomeService(IPropertyRepository propertyRepository, IAccountService accountService, IMapper mapper, IPropertyTypeRepository propertyTypeRepository, ISaleTypeRepository saletypeRepository, IImprovementRepository improvementRepository)
        {
            _propertyRepository = propertyRepository;
            _accountservice = accountService;
            _mapper = mapper;
            _propertyTypeRepository = propertyTypeRepository;
            _saletypeRepository = saletypeRepository;
            _improvementRepository = improvementRepository;
        }

        public async Task<AdminHomeViewModel> GetAdminHomeAsync()
        {
            AdminHomeViewModel vm = new AdminHomeViewModel();

            var properties = await _propertyRepository.GetAllAsync();
            
            if(properties != null && properties.Count > 0) 
            {
                vm.AllProperties = properties.Count();

            }

            var agents = await _accountservice.GetAllByRoleAsync(Roles.AGENTE.ToString());

            if(agents != null && agents.Count > 0)
            {
                vm.AllAgentsActive = agents.Where(u => u.EmailConfirmed && u.IsActive).ToList().Count();
                vm.AllAgentsInactive = agents.Where(u => !u.EmailConfirmed && !u.IsActive).ToList().Count();
            }

            var clients = await _accountservice.GetAllByRoleAsync(Roles.CLIENTE.ToString());

            if (clients != null && clients.Count > 0)
            {
                vm.AllClientsActive = clients.Where(u => u.EmailConfirmed && u.IsActive).ToList().Count();
                vm.AllClientsInactive = clients.Where(u => !u.EmailConfirmed && !u.IsActive).ToList().Count();
            }

            var developers = await _accountservice.GetAllByRoleAsync(Roles.DESARROLLADOR.ToString());

            if (developers != null && developers.Count > 0)
            {
                vm.AllDevelopersActive = developers.Where(u => u.EmailConfirmed && u.IsActive).ToList().Count();
                vm.AllDevelopersInactive = developers.Where(u => !u.EmailConfirmed && !u.IsActive).ToList().Count();
            }

            return vm;
        }

        public async Task<List<AdminHomeAgentViewModel>> GetAgentsAdminHomeAsync()
        {
            var agents = await _accountservice.GetAllByRoleAsync(Roles.AGENTE.ToString());

            List<AdminHomeAgentViewModel>  agentsvm = _mapper.Map<List<AdminHomeAgentViewModel>>(agents);

            foreach (var agent in agentsvm)
            {
                var agentsproperties = await _propertyRepository.GetAllByAgentIdAsync(agent.Id);
                agent.Properties = agentsproperties.Count();
            
            }

            return agentsvm;
        }

        public async Task<List<AdminHomeAdminViewModel>> GetAdminsAdminHomeAsync()
        {
            var admins = await _accountservice.GetAllByRoleAsync(Roles.ADMIN.ToString());

            List<AdminHomeAdminViewModel> adminsvm = _mapper.Map<List<AdminHomeAdminViewModel>>(admins);

            return adminsvm;
        }

        public async Task<List<AdminHomeDeveloperViewModel>> GetDevelopersAdminHomeAsync()
        {
            var developers = await _accountservice.GetAllByRoleAsync(Roles.DESARROLLADOR.ToString());

            List<AdminHomeDeveloperViewModel> developersvm = _mapper.Map<List<AdminHomeDeveloperViewModel>>(developers);

            return developersvm;
        }

        public async Task<List<AdminHomePropertyTypesViewModel>> GetPropertyTypesAdminHomeAsync()
        {

            var propertytypes = await _propertyTypeRepository.GetAllAsyncWithInclude(new List<string> { "Properties" });

            List<AdminHomePropertyTypesViewModel> propertytypesvm = _mapper.Map<List<AdminHomePropertyTypesViewModel>>(propertytypes);

            foreach (var propertytype in propertytypesvm)
            {
                
                propertytype.Propertiesquantity = propertytype.Properties.Count();

            }

            return propertytypesvm;
        }

        public async Task<List<AdminHomeSaleTypesViewModel>> GetSaleTypesAdminHomeAsync()
        {

            var saletypes = await _saletypeRepository.GetAllAsyncWithInclude(new List<string> { "Properties" });

            List<AdminHomeSaleTypesViewModel> saletypesvm = _mapper.Map<List<AdminHomeSaleTypesViewModel>>(saletypes);

            foreach (var saletype in saletypesvm)
            {

                saletype.Propertiesquantity = saletype.Properties.Count();

            }

            return saletypesvm;
        }

        public async Task<List<AdminHomeImprovementsViewModel>>GetImprovementssAdminHomeAsync()
        {

            var Improvements = await _improvementRepository.GetAllAsync();

            List<AdminHomeImprovementsViewModel> Improvementsvm = _mapper.Map<List<AdminHomeImprovementsViewModel>>(Improvements);

            return Improvementsvm;
        }


    }
}
