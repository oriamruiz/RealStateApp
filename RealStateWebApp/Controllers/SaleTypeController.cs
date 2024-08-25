using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.PropertyTypes;
using RealStateApp.Core.Application.ViewModels.SaleTypes;

namespace RealStateApp.WebApp.Controllers
{

    [Authorize(Roles = "ADMIN")]

    public class SaleTypeController : Controller
    {
        private readonly ISaleTypeService _saleTypeService;

        public SaleTypeController(ISaleTypeService saleTypeService)
        {
            _saleTypeService = saleTypeService;
        }

        public async Task<IActionResult> Create()
        {
            return View("SaveSaleType", new SaveSaleTypeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveSaleTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveSaleType", vm);
            }

            vm = await _saleTypeService.CreateViewModel(vm);

            if (vm.HasError)
            {
                return View("SaveSaleType", vm);
            }

            return RedirectToRoute(new { controller = "AdminHome", action = "SaleTypes" });
        }




        public async Task<IActionResult> Update(int Id)
        {

            return View("SaveSaleType", await _saleTypeService.GetByIdViewModel(Id));

        }

        [HttpPost]
        public async Task<IActionResult> Update(SaveSaleTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveSaleType", vm);
            }

            var response = await _saleTypeService.UpdateViewModel(vm, vm.Id);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("SaveSaleType", vm);
            }


            return RedirectToRoute(new { controller = "AdminHome", action = "SaleTypes" });

        }

        public async Task<IActionResult> Delete(int Id)
        {
            return View(await _saleTypeService.GetByIdViewModel(Id));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SaveSaleTypeViewModel vm)
        {

            var check = await _saleTypeService.CheckDelete(vm.Id);

            if (check.HasError)
            {
                vm.HasError = check.HasError;
                vm.Error = check.Error;
                return View(vm);
            }

            var propertiesid = await _saleTypeService.DeletePropertiesAssociatedViewModel(vm.Id);

            await _saleTypeService.DeleteViewModel(vm.Id);

            if (propertiesid != null && propertiesid.Count > 0)
            {
                foreach (var propid in propertiesid)
                {
                    DeleteImageDirectory(propid.ToString());
                }
            }


            return RedirectToRoute(new { controller = "AdminHome", action = "SaleTypes" });
        }


        #region private methods


        private void DeleteImageDirectory(string id)
        {

            string basepath = $"/images/Properties/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basepath}");

            if (Directory.Exists(path))
            {
                DirectoryInfo directoryinfo = new DirectoryInfo(path);

                foreach (FileInfo file in directoryinfo.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo folder in directoryinfo.GetDirectories())
                {
                    folder.Delete(true);
                }

                Directory.Delete(path);
            }

        }
        #endregion
    }
}
