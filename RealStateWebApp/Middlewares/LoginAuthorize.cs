using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Helpers.Sessions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.MiddleWare;

namespace RealStateApp.Middlewares
{
    public class LoginAuthorize : IAsyncActionFilter
    {
        private readonly ValidateUserSession _userSession;
        private readonly IHttpContextAccessor _httpContext;
        private readonly AuthenticationResponse _userViewModel;

        public LoginAuthorize(ValidateUserSession validateUser, IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
            _userSession = validateUser;
            _userViewModel = _httpContext.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_userSession.HasUser())
            {
                if (context.Controller is Controller controller)
                {
                    if (_userViewModel.Roles.Any(r => r == Roles.ADMIN.ToString()))
                    {
                        context.Result = controller.RedirectToAction("Index", "AdminHome");
                    }
                    else if (_userViewModel.Roles.Any(r => r == Roles.CLIENTE.ToString()))
                    {
                        context.Result = controller.RedirectToAction("Index", "ClientHome");
                    }
                    else if (_userViewModel.Roles.Any(r => r == Roles.AGENTE.ToString()))
                    {
                        context.Result = controller.RedirectToAction("Index", "AgentHome");
                    }
                    else
                    {
                        await next();
                    }
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
        }
    }
}
