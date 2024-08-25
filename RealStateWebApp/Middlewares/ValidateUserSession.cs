using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Helpers.Sessions;

namespace RealStateApp.MiddleWare
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContext;

        public ValidateUserSession(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public bool HasUser()
        {
            AuthenticationResponse userVm = _httpContext.HttpContext.Session.Get<AuthenticationResponse>("user");
            return userVm == null ? false : true;
        }
    }
}
