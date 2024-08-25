using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IPropertyService : IGenericService<PropertyViewModel, SavePropertyViewModel, Property>
    {

        Task<List<PropertyViewModel>> GetAllByAgentIdViewModel(string id);
        Task<SavePropertyViewModel> GetSavePropertyViewModelView(SavePropertyViewModel vm);
        Task UpdateImgViewModel(SavePropertyViewModel vm, int Id);
        Task<DeletePropertyViewModel> CheckDelete(int id);
        Task<DetailsPropertyViewModel> GetByIdDetailsViewModel(int id);

        Task ChangeFavoritePropertyStatusAsync(int propid);
        Task<List<PropertyViewModel>> GetAllMyFavoritePropertiesViewModel();
        Task<List<PropertyViewModel>> GetPropertiesByCodeViewModel(string code);
        Task<List<PropertyViewModel>> GetPropertiesByFiltersViewModel(FiltersPropertyViewModel filtersvm);
        Task<List<PropertyViewModel>> GetAgentPropertiesByCodeViewModel(string Id, string code);
        Task<List<PropertyViewModel>> GetAgentPropertiesByFiltersViewModel(FiltersPropertyViewModel filtersvm, string Id);
        Task<List<PropertyViewModel>> GetMyFavoritePropertiesByCodeViewModel(string code);
        Task<List<PropertyViewModel>> GetMyFavoritePropertiesByFiltersViewModel(FiltersPropertyViewModel filtersvm);

    }
}