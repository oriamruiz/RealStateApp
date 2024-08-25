using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.SaleTypes;
using RealStateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Services
{
    public class SaleTypeService : GenericService<SaleTypeViewModel, SaveSaleTypeViewModel, SaleType>, ISaleTypeService
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyService _propertyService;

        public SaleTypeService(ISaleTypeRepository repository, IHttpContextAccessor httpContext, IMapper mapper, IUserService user, IPropertyRepository propertyRepository, IPropertyService propertyService) : base(repository, mapper)
        {
            _saleTypeRepository = repository;
            _mapper = mapper;
            _userService = user;
            _propertyRepository = propertyRepository;
            _propertyService = propertyService;
        }

        public override async Task<SaveSaleTypeViewModel> CreateViewModel(SaveSaleTypeViewModel vm)
        {
            var proptypes = await _saleTypeRepository.GetAllAsync();

            var nameduplicated = proptypes.FirstOrDefault(pt => pt.Name.ToLower() == vm.Name.ToLower());

            if (nameduplicated != null)
            {
                vm.HasError = true;
                vm.Error = "Ya se ha creado un tipo de venta con este nombre";
                return vm;
            }


            return await base.CreateViewModel(vm); ;
        }

        public override async Task<SaveSaleTypeViewModel> UpdateViewModel(SaveSaleTypeViewModel vm, int Id)
        {
            var proptypes = await _saleTypeRepository.GetAllAsync();

            var nameduplicated = proptypes.FirstOrDefault(pt => pt.Name.ToLower() == vm.Name.ToLower());

            if (nameduplicated != null && nameduplicated.Id != Id)
            {
                vm.HasError = true;
                vm.Error = "Ya existe un tipo de venta con este nombre. no es posible actualizar.";
                return vm;
            }


            return await base.UpdateViewModel(vm, Id); ;
        }

        public async Task<SaveSaleTypeViewModel> CheckDelete(int id)
        {
            SaveSaleTypeViewModel vm = new();
            vm.HasError = false;

            var propertyTypeToDelete = await _saleTypeRepository.GetByIdAsync(id);

            if (propertyTypeToDelete == null)
            {
                vm.HasError = true;
                vm.Error = "El tipo de venta que intentas borrar no existe";
            }

            return vm;
        }

        public async Task<List<int>> DeletePropertiesAssociatedViewModel(int id)
        {
            List<int> propertiesId = null;

            var properties = await _propertyRepository.GetAllAsync();

            var propertiesWhithThisType = properties.Where(p => p.SaleTypeId == id).ToList();

            if (propertiesWhithThisType != null && propertiesWhithThisType.Count > 0)
            {
                propertiesId = propertiesWhithThisType.Select(p => p.Id).ToList();

                foreach (var property in propertiesWhithThisType)
                {
                    await _propertyService.DeleteViewModel(property.Id);
                }
            }

            return propertiesId;
        }
    }
}
