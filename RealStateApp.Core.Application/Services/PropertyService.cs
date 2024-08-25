using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Helpers.Sessions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.Core.Domain.Entities;


namespace RealStateApp.Core.Application.Services
{
    public class PropertyService : GenericService<PropertyViewModel, SavePropertyViewModel, Property>, IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IPropertyImprovementRepository _propertyImprovementRepository;
        private readonly IFavoritePropertyRepository _favoritePropertyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _user;

        public PropertyService(IPropertyRepository repository, IHttpContextAccessor httpContext, IMapper mapper, IUserService user, IPropertyTypeRepository propertyTypeRepository, ISaleTypeRepository saleTypeRepository, IImprovementRepository improvementRepository, IPropertyImprovementRepository propertyImprovementRepository, IHttpContextAccessor httpContextAccessor, IFavoritePropertyRepository favoritePropertyRepository) : base(repository, mapper)
        {
            _propertyRepository = repository;
            _mapper = mapper;
            _userService = user;
            _propertyTypeRepository = propertyTypeRepository;
            _saleTypeRepository = saleTypeRepository;
            _improvementRepository = improvementRepository;
            _propertyImprovementRepository = propertyImprovementRepository;
            _httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _favoritePropertyRepository = favoritePropertyRepository;
        }

        public override async Task<SavePropertyViewModel> CreateViewModel(SavePropertyViewModel vm)
        {
            Random random = new Random();
            bool done = false;

            while (!done)
            {
                var code = random.Next(100000, 1000000).ToString();

                var posibleCodeDuplicate = await _propertyRepository.GetByCodeAsync(code);

                if (posibleCodeDuplicate == null)
                {
                    vm.Code = code;
                    done = true;
                }

            }

            var propertycreated = await base.CreateViewModel(vm);

            if(propertycreated.Id > 0 )
            {
                vm.Id = propertycreated.Id;

                foreach (var improvement in vm.ImprovementsIds)
                {
                    var imp = await _improvementRepository.GetByIdAsync(improvement);

                    if (imp != null)
                    {
                        PropertyImprovement propertyImprovement = new PropertyImprovement();
                        propertyImprovement.PropertyId = propertycreated.Id;
                        propertyImprovement.ImprovementId = imp.Id;

                        await _propertyImprovementRepository.AddAsync(propertyImprovement);

                    }

                }
            }
            else
            {
                vm.HasError = true;
                vm.Error = "No se pudo crear la propiedad.";

            }

            return vm;
        }

        public override async Task<SavePropertyViewModel> UpdateViewModel(SavePropertyViewModel vm, int Id)
        {
            var properies = await _propertyRepository.GetAllAsyncWithInclude(new List<string> { "Improvements" });

            var property = properies.FirstOrDefault(p => p.Id == Id);

            if(property != null)
            {

                if (vm.AgentId != _user.Id)
                {
                    vm.HasError = true;
                    vm.Error = "Estas tratando de enviar un id que no es el tuyo";
                    return vm;
                }

                if (property.AgentId != _user.Id)
                {
                    vm.HasError = true;
                    vm.Error = "no eres dueño de la propiedad que estas tratando de editar";
                }
                

                if (vm.Code != property.Code || vm.MainImageUrl != property.MainImageUrl || vm.ImageUrl2 != property.ImageUrl2 || vm.ImageUrl3 != property.ImageUrl3 || vm.ImageUrl4 != property.ImageUrl4)
                {
                    vm.HasError = true;
                    vm.Error += "Ha ocurrido un error inesperado. favor de no intentar atacar la integridad de la web.";
                }

                if(!vm.HasError)
                {

                    if(property.Improvements != null && property.Improvements.Count> 0) {

                        var actualimprovements = property.Improvements;

                        var improvementsToRemove = new List<PropertyImprovement>();

                        foreach (var actualpropimp in actualimprovements)
                        {
                            if (!vm.ImprovementsIds.Contains(actualpropimp.ImprovementId))
                            {
                                improvementsToRemove.Add(actualpropimp);
                            }
                        }

                        foreach (var improvementToRemove in improvementsToRemove)
                        {
                            await _propertyImprovementRepository.DeleteAsync(improvementToRemove);
                        }

                        foreach (var improvementid in vm.ImprovementsIds)
                        {
                            if (!actualimprovements.Any(i => i.ImprovementId == improvementid))
                            {
                                var improvement = await _improvementRepository.GetByIdAsync(improvementid);
                                if (improvement != null)
                                {
                                    PropertyImprovement propertyImprovement = new();
                                    propertyImprovement.PropertyId = property.Id;
                                    propertyImprovement.ImprovementId = improvementid;


                                    await _propertyImprovementRepository.AddAsync(propertyImprovement);

                                }
                                else
                                {
                                    vm.HasError = true;
                                    vm.Error += $"la mejora con id {improvementid} no existe.";
                                }
                            }

                        }

                    }
                    else
                    {
                        foreach (var improvementid in vm.ImprovementsIds)
                        {
                            
                            var improvement = await _improvementRepository.GetByIdAsync(improvementid);
                            if (improvement != null)
                            {
                                PropertyImprovement propertyImprovement = new();
                                propertyImprovement.PropertyId = property.Id;
                                propertyImprovement.ImprovementId = improvementid;


                                await _propertyImprovementRepository.AddAsync(propertyImprovement);

                            }
                            else
                            {
                                vm.HasError = true;
                                vm.Error += $"la mejora con id {improvementid} no existe.";
                            }

                        }
                    }

                   


                    vm = await base.UpdateViewModel(vm, vm.Id);
                }

            }
            else
            {
                vm.HasError = true;
                vm.Error = "la propiedad que intentas editar no existe.";
            }

            return vm;
        }


        public override async Task<List<PropertyViewModel>> GetAllViewModel()
        {

            var vm = await base.GetAllViewModel();

            if(_user != null && _user.Roles.Contains(Roles.CLIENTE.ToString()))
            {
                
                foreach (var property in vm)
                {
                    var propertiesfav = await _favoritePropertyRepository.GetAllAsyncWithInclude(new List<string> { "Property" });

                    var isFav = propertiesfav.FirstOrDefault(pf => pf.PropertyId == property.Id && pf.ClientId == _user.Id);

                    if (isFav != null)
                    {
                        property.IsFavorite = true;
                    }
                }

            }


            return vm;
        }

        public async Task<List<PropertyViewModel>> GetPropertiesByCodeViewModel(string code)
        {
            var propertiesvm = await GetAllViewModel();

            if(propertiesvm != null)
            {
                if (!string.IsNullOrEmpty(code))
                {
                    propertiesvm = propertiesvm.Where(p => p.Code == code).ToList();
                }
            }

            return propertiesvm;
        }

        

        public async Task<List<PropertyViewModel>> GetPropertiesByFiltersViewModel(FiltersPropertyViewModel filtersvm)
        {
            var propertiesvm = await GetAllViewModel();

            if (propertiesvm != null)
            {
                if (filtersvm.PropertyTypeIdfilter > 0)
                {
                    propertiesvm = propertiesvm.Where(p => p.PropertyTypeId == filtersvm.PropertyTypeIdfilter).ToList();
                    
                }


                if(filtersvm.MinPrice>=0 && filtersvm.MaxPrice >=0) 
                {
                    if (filtersvm.MinPrice < filtersvm.MaxPrice)
                    {
                        propertiesvm = propertiesvm.Where(p=> p.Price >= filtersvm.MinPrice && p.Price <= filtersvm.MaxPrice).ToList();
                    }

                }

                if (filtersvm.BedromsQuantity > 0)
                {
                    propertiesvm = propertiesvm.Where(p=> p.Bedrooms == filtersvm.BedromsQuantity).ToList();

                }

                if(filtersvm.BathroomsQuantity > 0)
                {
                    propertiesvm = propertiesvm.Where(p => p.Bathrooms == filtersvm.BathroomsQuantity).ToList();

                }

            }

            

            return propertiesvm;
        }

        public async Task<List<PropertyViewModel>> GetAgentPropertiesByCodeViewModel(string code, string Id)
        {
            var propertiesvm = await GetAllByAgentIdViewModel(Id);

            if (propertiesvm != null)
            {
                if (!string.IsNullOrEmpty(code))
                {
                    propertiesvm = propertiesvm.Where(p => p.Code == code).ToList();
                }
            }

            return propertiesvm;
        }

        public async Task<List<PropertyViewModel>> GetAgentPropertiesByFiltersViewModel(FiltersPropertyViewModel filtersvm, string Id)
        {
            var propertiesvm = await GetAllByAgentIdViewModel(Id);

            if (propertiesvm != null)
            {
                if (filtersvm.PropertyTypeIdfilter > 0)
                {
                    propertiesvm = propertiesvm.Where(p => p.PropertyTypeId == filtersvm.PropertyTypeIdfilter).ToList();

                }


                if (filtersvm.MinPrice >= 0 && filtersvm.MaxPrice >= 0)
                {
                    if (filtersvm.MinPrice < filtersvm.MaxPrice)
                    {
                        propertiesvm = propertiesvm.Where(p => p.Price >= filtersvm.MinPrice && p.Price <= filtersvm.MaxPrice).ToList();
                    }

                }

                if (filtersvm.BedromsQuantity > 0)
                {
                    propertiesvm = propertiesvm.Where(p => p.Bedrooms == filtersvm.BedromsQuantity).ToList();

                }

                if (filtersvm.BathroomsQuantity > 0)
                {
                    propertiesvm = propertiesvm.Where(p => p.Bathrooms == filtersvm.BathroomsQuantity).ToList();

                }

            }



            return propertiesvm;
        }

        public async Task<List<PropertyViewModel>> GetMyFavoritePropertiesByCodeViewModel(string code)
        {
            var propertiesvm = await GetAllMyFavoritePropertiesViewModel();

            if (propertiesvm != null)
            {
                if (!string.IsNullOrEmpty(code))
                {
                    propertiesvm = propertiesvm.Where(p => p.Code == code).ToList();
                }
            }

            return propertiesvm;
        }


        public async Task<List<PropertyViewModel>> GetMyFavoritePropertiesByFiltersViewModel(FiltersPropertyViewModel filtersvm)
        {
            var propertiesvm = await GetAllMyFavoritePropertiesViewModel();

            if (propertiesvm != null)
            {
                if (filtersvm.PropertyTypeIdfilter > 0)
                {
                    propertiesvm = propertiesvm.Where(p => p.PropertyTypeId == filtersvm.PropertyTypeIdfilter).ToList();

                }


                if (filtersvm.MinPrice >= 0 && filtersvm.MaxPrice >= 0)
                {
                    if (filtersvm.MinPrice < filtersvm.MaxPrice)
                    {
                        propertiesvm = propertiesvm.Where(p => p.Price >= filtersvm.MinPrice && p.Price <= filtersvm.MaxPrice).ToList();
                    }

                }

                if (filtersvm.BedromsQuantity > 0)
                {
                    propertiesvm = propertiesvm.Where(p => p.Bedrooms == filtersvm.BedromsQuantity).ToList();

                }

                if (filtersvm.BathroomsQuantity > 0)
                {
                    propertiesvm = propertiesvm.Where(p => p.Bathrooms == filtersvm.BathroomsQuantity).ToList();

                }

            }



            return propertiesvm;
        }

        public override async Task DeleteViewModel(int id)
        {

            var propertyToDelete = await _propertyRepository.GetByIdAsync(id);

            if (propertyToDelete != null)
            {
                var improvements = await _propertyImprovementRepository.GetAllAsync();

                var propertyimprovementstodelete = improvements.Where(pi => pi.PropertyId == propertyToDelete.Id).ToList();

                if(propertyimprovementstodelete != null && propertyimprovementstodelete.Count > 0)
                {
                    foreach (var imp in propertyimprovementstodelete)
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

                await base.DeleteViewModel(id);

            }
            
        }


        public async Task UpdateImgViewModel(SavePropertyViewModel vm, int Id)
        { 
            await base.UpdateViewModel(vm, vm.Id);
        }

        public async Task<DeletePropertyViewModel> CheckDelete(int id)
        {
            DeletePropertyViewModel vm = new();
            vm.HasError = false;

            var propertyToDelete = await _propertyRepository.GetByIdAsync(id);

            if (propertyToDelete != null)
            {
                if(propertyToDelete.AgentId != _user.Id)
                {
                    vm.HasError = true;
                    vm.Error = "Esta propiedad no es tuya, no es posible borrarla";
                }

            }
            else
            {
                vm.HasError= true;
                vm.Error = "La propiedad que intentas borrar no existe";
            }


            return vm;
        }


        public async Task<List<PropertyViewModel>> GetAllByAgentIdViewModel(string Id)
        {
            List<PropertyViewModel> propertyViewModel = null;

            var user = _userService.GetByIdAsync(Id);

            if(user != null)
            {
                var agentproperties = await _propertyRepository.GetAllByAgentIdAsync(Id);
                propertyViewModel = _mapper.Map<List<PropertyViewModel>>(agentproperties);
            }

            return propertyViewModel;
        }

        public async Task<SavePropertyViewModel> GetSavePropertyViewModelView(SavePropertyViewModel vm)
        {
            var agent =  await _userService.GetByIdAsync(vm.AgentId);

            if (agent.HasError)
            {
                vm.HasError = true;
                vm.Error = "El agente que desea hacer esta operacion no se encontro.";
                return vm;
            }
            else
            {

                if (vm.Id > 0)
                {
                    var properties = await _propertyRepository.GetAllAsyncWithInclude(new List<string> { "Improvements" });
                    var property = properties.FirstOrDefault(p => p.Id == vm.Id);

                    if (property == null)
                    {
                        vm.HasError = true;
                        vm.Error += "No se encontro la propiedad a editar";
                        return vm;
                    }
                    else
                    {
                        List<int> improvementsId = property.Improvements.Select(imp => imp.ImprovementId).ToList();

                        vm = _mapper.Map<SavePropertyViewModel>(property);

                        vm.ImprovementsIds = improvementsId;

                    }


                }

                vm.PropertyTypes = await _propertyTypeRepository.GetAllAsync();
                vm.Improvements = await _improvementRepository.GetAllAsync();
                vm.SaleTypes = await _saleTypeRepository.GetAllAsync();
                
                if(vm.PropertyTypes == null || vm.PropertyTypes.Count == 0)
                {
                    vm.HasError = true;
                    vm.Error += "Error: No hay tipos de propiedades creadas. ";
                }

                if (vm.SaleTypes == null || vm.SaleTypes.Count == 0)
                {
                    vm.HasError = true;
                    vm.Error += "Error: No hay tipos de ventas creadas. ";
                }

                if (vm.Improvements == null || vm.Improvements.Count == 0)
                {
                    vm.HasError = true;
                    vm.Error += "Error: No hay mejoras creadas.";
                }
            }

            

            return vm;
        }


        public async Task<DetailsPropertyViewModel> GetByIdDetailsViewModel(int id)
        {
            DetailsPropertyViewModel vm = new();

            var properties = await _propertyRepository.GetAllAsyncWithInclude(new List<string>{"Improvements", "PropertyType","SaleType"});
            var property = properties.FirstOrDefault(p=> p.Id == id);

            if(property != null)
            {
                vm = _mapper.Map<DetailsPropertyViewModel>(property);


                if (!string.IsNullOrEmpty(property.ImageUrl2))
                    vm.OptionalImages.Add(property.ImageUrl2);

                if (!string.IsNullOrEmpty(property.ImageUrl3))
                    vm.OptionalImages.Add(property.ImageUrl3);

                if (!string.IsNullOrEmpty(property.ImageUrl4))
                    vm.OptionalImages.Add(property.ImageUrl4);

                var agent = await _userService.GetByIdAsync(vm.AgentId);
                vm.Agent = agent;

                foreach(var imp in vm.Improvements)
                {
                    imp.Improvement = await _improvementRepository.GetByIdAsync(imp.ImprovementId);
                }

            }
            else
            {
                vm.HasError = true;
                vm.Error = "No se encontro esta propiedad";
            }
            

            return vm;


        }

        public async Task ChangeFavoritePropertyStatusAsync(int propid)
        {
            var prop = await base.GetByIdViewModel(propid);

            var favoritesproperties = await _favoritePropertyRepository.GetAllAsync();
            
            var favoritepropertie = favoritesproperties.FirstOrDefault(f => f.PropertyId == propid && f.ClientId == _user.Id);
        
            if (favoritepropertie != null)
            {
                await _favoritePropertyRepository.DeleteAsync(favoritepropertie);
            }
            else
            {
                FavoriteProperty favoriteProperty = new();
                favoriteProperty.PropertyId = propid;
                favoriteProperty.ClientId = _user.Id;

                await _favoritePropertyRepository.AddAsync(favoriteProperty);
            }
        
        }


        public async Task<List<PropertyViewModel>> GetAllMyFavoritePropertiesViewModel()
        {

            List<PropertyViewModel> vm = new();

            var properties = await base.GetAllViewModel();

            if (_user != null)
            {
                foreach (var property in properties)
                {
                    var propertiesfav = await _favoritePropertyRepository.GetAllAsyncWithInclude(new List<string> { "Property" });

                    var isFav = propertiesfav.FirstOrDefault(pf => pf.PropertyId == property.Id && pf.ClientId == _user.Id);

                    if (isFav != null)
                    {
                        vm.Add(property);
                        
                    }
                }

            }



            return vm;
        }



    }
}
