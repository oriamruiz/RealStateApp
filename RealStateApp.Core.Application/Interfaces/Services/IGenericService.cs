

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IGenericService<ViewModel, SaveViewModel, Entity>
        where ViewModel : class
        where SaveViewModel : class
        where Entity : class
    {
        Task<List<ViewModel>> GetAllViewModel();
        Task<SaveViewModel> CreateViewModel(SaveViewModel vm);
        Task<SaveViewModel> GetByIdViewModel(int id);

        Task<SaveViewModel> UpdateViewModel(SaveViewModel vm, int id);

        Task DeleteViewModel(int id);
    }
}
