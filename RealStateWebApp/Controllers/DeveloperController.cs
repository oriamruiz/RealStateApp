using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Developers;
using RealStateApp.Core.Application.ViewModels.Users;

namespace RealStateApp.WebApp.Controllers
{
    [Authorize(Roles = "ADMIN")]

    public class DeveloperController : Controller
    {
        private readonly IUserService _userService;

        public DeveloperController(IUserService userService)
        {
            _userService = userService;
        }


        public async Task<IActionResult> ChangeStatus(string id)
        {
            return View(await _userService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatusPost(string Id)
        {
            var response = await _userService.ChangeStatusUser(Id);
            if (response.HasError)
            {
                return View("ChangeStatus", response);
            }

            return RedirectToRoute(new { controller = "AdminHome", action = "Developers" });
        }

        public async Task<IActionResult> Register()
        {
            return View(new CreateAdminOrDeveloperViewModel());
        }


        [HttpPost]

        public async Task<IActionResult> Register(CreateAdminOrDeveloperViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            vm.Role = Roles.DESARROLLADOR.ToString();
            RegisterResponse response = await _userService.RegisterAdminOrDeveloperAsync(vm);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }

            return RedirectToRoute(new { controller = "AdminHome", action = "Developers" });

        }

        public async Task<IActionResult> Update(string Id)
        {
            return View(await _userService.GetByIdUpdateDeveloperViewModelAsync(Id));
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateDeveloperViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var response = await _userService.UpdateDeveloperAsync(vm);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }


            return RedirectToRoute(new { controller = "AdminHome", action = "Developers" });

        }
    }
}
