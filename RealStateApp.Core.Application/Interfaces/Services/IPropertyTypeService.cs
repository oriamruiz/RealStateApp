using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels.PropertyTypes;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IPropertyTypeService : IGenericService<PropertyTypeViewModel, SavePropertyTypeViewModel, PropertyType>
    {
        Task<SavePropertyTypeViewModel> CheckDelete(int id);

        Task<List<int>> DeletePropertiesAssociatedViewModel(int id);
    }
}