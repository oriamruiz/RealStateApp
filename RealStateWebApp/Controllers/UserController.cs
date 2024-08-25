using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Helpers.Sessions;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Users;
using RealStateApp.Middlewares;

namespace RealStateApp.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }



        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Index()
        {
     
            return View(new LoginViewModel());
        }


        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AuthenticationResponse userVm = await _userService.LoginAsync(vm);

            if (userVm != null && userVm.HasError != true)
            {
                HttpContext.Session.Set<AuthenticationResponse>("user", userVm);

                var controllerToRedirect = await _userService.RedirectByRoleAsync(userVm.Roles);

                return RedirectToRoute(new { controller = controllerToRedirect, action = "Index" });

            }
            else
            {
                vm.HasError = userVm.HasError;
                vm.Error = userVm.Error;
                return View(vm);
            }

        }



        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }


        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> Register()
        {
            return View(new CreateAgentOrClientViewModel());
        }


        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        
        public async Task<IActionResult> Register(CreateAgentOrClientViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var origin = Request.Headers["origin"];
            RegisterResponse response = await _userService.RegisterClientOrAgentAsync(vm, origin);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }

            vm.Id = response.Id;
            vm.AccountImgUrl = UploadFile(vm.ImageFile, vm.Id);
            await _userService.UpdateImgViewModel(new UpdateImgViewModel { Id = vm.Id, AccountImgUrl = vm.AccountImgUrl});

            return RedirectToRoute(new { controller = "User", action = "Index" });

        }

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            string response = await _userService.ConfirmEmailAsync(userId, token);

            return View("ConfirmEmail", response);
        }

        #region private methods
        private string UploadFile(IFormFile file, string id)
        {
            string basepath = $"/images/Users/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basepath}");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Guid guid = Guid.NewGuid();
            FileInfo fileinfo = new(file.FileName);
            string filename = guid + fileinfo.Extension;

            string filenamewithpath = Path.Combine(path, filename);

            using (var stream = new FileStream(filenamewithpath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return $"{basepath}/{filename}";

        }
        #endregion

    }
}
