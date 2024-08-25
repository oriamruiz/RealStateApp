using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Improvements;
using RealStateApp.Core.Application.ViewModels.SaleTypes;

namespace RealStateApp.WebApp.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class ImprovementController : Controller
    {
        private readonly IImprovementService _improvementService;

        public ImprovementController(IImprovementService improvementService)
        {
            _improvementService = improvementService;
        }

        public async Task<IActionResult> Create()
        {
            return View("SaveImprovement", new SaveImprovementViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveImprovementViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveImprovement", vm);
            }

            vm = await _improvementService.CreateViewModel(vm);

            if (vm.HasError)
            {
                return View("SaveImprovement", vm);
            }

            return RedirectToRoute(new { controller = "AdminHome", action = "Improvements" });
        }




        public async Task<IActionResult> Update(int Id)
        {

            return View("SaveImprovement", await _improvementService.GetByIdViewModel(Id));

        }

        [HttpPost]
        public async Task<IActionResult> Update(SaveImprovementViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveImprovement", vm);
            }

            var response = await _improvementService.UpdateViewModel(vm, vm.Id);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("SaveImprovement", vm);
            }


            return RedirectToRoute(new { controller = "AdminHome", action = "Improvements" });

        }

        public async Task<IActionResult> Delete(int Id)
        {
            return View(await _improvementService.GetByIdViewModel(Id));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SaveImprovementViewModel vm)
        {
            var check = await _improvementService.CheckDelete(vm.Id);

            if (check.HasError)
            {
                vm.HasError = check.HasError;
                vm.Error = check.Error;
                return View(vm);
            }

            await _improvementService.DeleteViewModel(vm.Id);

            return RedirectToRoute(new { controller = "AdminHome", action = "Improvements" });
        }
    }
}
