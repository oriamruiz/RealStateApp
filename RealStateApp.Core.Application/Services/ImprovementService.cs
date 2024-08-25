using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Improvements;
using RealStateApp.Core.Application.ViewModels.SaleTypes;
using RealStateApp.Core.Domain.Entities;


namespace RealStateApp.Core.Application.Services
{
    public class ImprovementService : GenericService<ImprovementViewModel, SaveImprovementViewModel, Improvement>, IImprovementService
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IPropertyImprovementRepository _propertyImprovementRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public ImprovementService(IImprovementRepository repository, IHttpContextAccessor httpContext, IMapper mapper, IUserService user, IPropertyImprovementRepository propertyImprovementRepository) : base(repository, mapper)
        {
            _improvementRepository = repository;
            _mapper = mapper;
            _userService = user;
            _propertyImprovementRepository = propertyImprovementRepository;

        }

        public override async Task<SaveImprovementViewModel> CreateViewModel(SaveImprovementViewModel vm)
        {
            var improvements = await _improvementRepository.GetAllAsync();

            var nameduplicated = improvements.FirstOrDefault(pt => pt.Name.ToLower() == vm.Name.ToLower());

            if (nameduplicated != null)
            {
                vm.HasError = true;
                vm.Error = "Ya se ha creado uns mejora con este nombre";
                return vm;
            }


            return await base.CreateViewModel(vm); ;
        }

        public override async Task<SaveImprovementViewModel> UpdateViewModel(SaveImprovementViewModel vm, int Id)
        {
            var improvements = await _improvementRepository.GetAllAsync();

            var nameduplicated = improvements.FirstOrDefault(pt => pt.Name.ToLower() == vm.Name.ToLower());

            if (nameduplicated != null && nameduplicated.Id != Id)
            {
                
                vm.HasError = true;
                vm.Error = "Ya existe una mejora con este nombre. no es posible actualizar.";
                return vm;
                
               
            }


            return await base.UpdateViewModel(vm, Id); 
        }

        public override async Task DeleteViewModel(int id)
        {
            var Propertyimprovements = await _propertyImprovementRepository.GetAllAsync();

            var PropertyimprovementsToDelete = Propertyimprovements.Where(pi => pi.ImprovementId == id).ToList();

            if (PropertyimprovementsToDelete != null && PropertyimprovementsToDelete.Count > 0)
            {
                foreach (var imp in PropertyimprovementsToDelete)
                {
                    await _propertyImprovementRepository.DeleteAsync(imp);
                }
            }

            await base.DeleteViewModel(id); 
        }

        public async Task<SaveImprovementViewModel> CheckDelete(int id)
        {
            SaveImprovementViewModel vm = new();
            vm.HasError = false;

            var improvementToDelete = await _improvementRepository.GetByIdAsync(id);

            if (improvementToDelete == null)
            {
                vm.HasError = true;
                vm.Error = "La mejora que intentas borrar no existe";
            }

            return vm;
        }
    }
}
