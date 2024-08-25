using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.ViewModels.Users;
using RealStateApp.Core.Application.ViewModels.Agents;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.ViewModels.Admins;
using RealStateApp.Core.Application.Dtos.Admin;
using RealStateApp.Core.Application.Dtos.Developer;
using RealStateApp.Core.Application.ViewModels.Developers;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<AuthenticationResponse> LoginAsync(LoginViewModel vm);
        Task<AuthenticationResponse> LoginByApiAsync(AuthenticationRequest request);
        Task<RegisterResponse> RegisterClientOrAgentAsync(CreateAgentOrClientViewModel vm, string origin);
        Task<RegisterResponse> RegisterAdminOrDeveloperAsync(CreateAdminOrDeveloperViewModel vm);
        Task SignOutAsync();
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<string> RedirectByRoleAsync(List<string> role);

        Task<UserViewModel> GetByIdAsync(string id);
        Task<UpdateAgentResponse> UpdateAgentAsync(UpdateAgentViewModel vm);
        Task<UpdateAdminResponse> UpdateAdminAsync(UpdateAdminViewModel vm);
        Task<UpdateDeveloperResponse> UpdateDeveloperAsync(UpdateDeveloperViewModel vm);
        Task UpdateImgViewModel(UpdateImgViewModel vm);

        Task<List<GetActiveAgentViewModel>> GetAllActiveAgentsAsync();

        Task<List<GetActiveAgentViewModel>> GetAgentsByNameViewModel(string Name);
        //Task<List<GetUserResponse>> GetAllAsync();
        //Task<UpdateResponse> Update(UpdateUserViewModel vm);

        Task<UserViewModel> ChangeStatusUser(string id);
        //Task<GetUserResponse> GetByIdAsync(string id);
        Task<UpdateAgentViewModel> GetByIdUpdateAgentViewModelAsync(string id);

        
        Task<DeleteAgentViewModel> DeleteAgentAsync(string Id);

        Task<UpdateAdminViewModel> GetByIdUpdateAdminViewModelAsync(string id);
        Task<UpdateDeveloperViewModel> GetByIdUpdateDeveloperViewModelAsync(string id);
    }

}