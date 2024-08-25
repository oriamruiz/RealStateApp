using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.Core.Application.ViewModels.PropertyTypes;
using RealStateApp.Core.Domain.Entities;
using System.Text.RegularExpressions;


namespace RealStateApp.Core.Application.Services
{
    public class PropertyTypeService : GenericService<PropertyTypeViewModel, SavePropertyTypeViewModel, PropertyType>, IPropertyTypeService
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyService _propertyService;

        public PropertyTypeService(IPropertyTypeRepository repository, IHttpContextAccessor httpContext, IMapper mapper, IUserService user, IPropertyRepository propertyRepository, IPropertyService propertyService) : base(repository, mapper)
        {
            _propertyTypeRepository = repository;
            _mapper = mapper;
            _userService = user;
            _propertyRepository = propertyRepository;
            _propertyService = propertyService;
        }

        public override async Task<SavePropertyTypeViewModel> CreateViewModel(SavePropertyTypeViewModel vm)
        {
            var proptypes = await _propertyTypeRepository.GetAllAsync();

            var nameduplicated = proptypes.FirstOrDefault(pt => pt.Name.ToLower() == vm.Name.ToLower());

            if (nameduplicated != null)
            {
                vm.HasError = true;
                vm.Error = "Ya se ha creado un tipo de propiedad con este nombre";
                return vm;
            }


            return await base.CreateViewModel(vm); ;
        }

        public override async Task<SavePropertyTypeViewModel> UpdateViewModel(SavePropertyTypeViewModel vm, int Id)
        {
            var proptypes = await _propertyTypeRepository.GetAllAsync();

            var nameduplicated = proptypes.FirstOrDefault(pt => pt.Name.ToLower() == vm.Name.ToLower());

            if (nameduplicated != null && nameduplicated.Id != Id)
            {
                vm.HasError = true;
                vm.Error = "Ya existe un tipo de propiedad con este nombre. no es posible actualizar.";
                return vm;
            }


            return await base.UpdateViewModel(vm, Id); ;
        }

        public async Task<SavePropertyTypeViewModel> CheckDelete(int id)
        {
            SavePropertyTypeViewModel vm = new();
            vm.HasError = false;

            var propertyTypeToDelete = await _propertyTypeRepository.GetByIdAsync(id);

            if (propertyTypeToDelete == null)
            {
                vm.HasError = true;
                vm.Error = "El tipo de propiedad que intentas borrar no existe";
            }

            return vm;
        }

        public async Task<List<int>> DeletePropertiesAssociatedViewModel(int id)
        {
            List<int> propertiesId = null;

            var properties = await _propertyRepository.GetAllAsync();

            var properttiesWhithThisType = properties.Where(p => p.PropertyTypeId == id).ToList();

            if(properttiesWhithThisType != null && properttiesWhithThisType.Count >0)
            {
                propertiesId = properttiesWhithThisType.Select(p => p.Id).ToList();
                
                foreach (var property in properttiesWhithThisType)
                {
                    await _propertyService.DeleteViewModel(property.Id);
                }
            }

            return propertiesId;
        }


    }
}
