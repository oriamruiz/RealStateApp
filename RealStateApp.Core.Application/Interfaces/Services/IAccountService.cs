using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.Admin;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Dtos.Developer;

namespace RealStateApp.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<RegisterResponse> RegisterAsync(RegisterRequest request, string origin ="");
        Task SignOutAsync();
        Task<GetUserResponse> GetById(string Id);
        Task<UpdateAgentResponse> UpdateAgentAsync(UpdateAgentRequest request);
        Task<UpdateAdminResponse> UpdateAdminAsync(UpdateAdminRequest request);
        Task<UpdateDeveloperResponse> UpdateDeveloperAsync(UpdateDeveloperRequest request);
        Task UpdateImgAsync(UpdateImageRequest request);
        Task<List<GetActiveAgentResponse>> GetAllActiveAgentsAsync();
        Task<UpdateAgentResponse> GetByIdAgentUpdateViewModelAsync(string Id);
        Task<UpdateAdminResponse> GetByIdAdminUpdateViewModelAsync(string Id);
        Task<UpdateDeveloperResponse> GetByIdDeveloperUpdateViewModelAsync(string Id);
        Task<List<GetUserResponse>> GetAllByRoleAsync(string role);
        Task<ChangeStatusResponse> ChangeStatusAsync(ChangeStatusRequest request);
        Task DeleteAgentAsync(string Id);
        Task<AuthenticationResponse> AuthenticateByApiAsync(AuthenticationRequest request);


    }
}