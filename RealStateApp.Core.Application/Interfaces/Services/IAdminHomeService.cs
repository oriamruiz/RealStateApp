using RealStateApp.Core.Application.ViewModels.AdminHome;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IAdminHomeService
    {
        Task<AdminHomeViewModel> GetAdminHomeAsync();
        Task<List<AdminHomeAgentViewModel>> GetAgentsAdminHomeAsync();

        Task<List<AdminHomeAdminViewModel>> GetAdminsAdminHomeAsync();
        Task<List<AdminHomeDeveloperViewModel>> GetDevelopersAdminHomeAsync();
        Task<List<AdminHomePropertyTypesViewModel>> GetPropertyTypesAdminHomeAsync();
        Task<List<AdminHomeSaleTypesViewModel>> GetSaleTypesAdminHomeAsync();
        Task<List<AdminHomeImprovementsViewModel>> GetImprovementssAdminHomeAsync();
    }
}