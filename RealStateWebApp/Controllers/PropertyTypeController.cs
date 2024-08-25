using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Agents;
using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.Core.Application.ViewModels.PropertyTypes;

namespace RealStateApp.WebApp.Controllers
{
    [Authorize(Roles = "ADMIN")]

    public class PropertyTypeController : Controller
    {

        private readonly IPropertyTypeService _propertyTypeService;

        public PropertyTypeController(IPropertyTypeService propertyTypeService)
        {
            _propertyTypeService = propertyTypeService;
        }

        public async Task<IActionResult> Create()
        {
            return View("SavePropertyType", new SavePropertyTypeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePropertyTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SavePropertyType", vm);
            }

            vm = await _propertyTypeService.CreateViewModel(vm);

            if (vm.HasError)
            {
                return View("SavePropertyType", vm);
            }

            return RedirectToRoute(new { controller = "AdminHome", action = "PropertyTypes" });
        }




        public async Task<IActionResult> Update(int Id)
        {

            return View("SavePropertyType", await _propertyTypeService.GetByIdViewModel(Id));

        }

        [HttpPost]
        public async Task<IActionResult> Update(SavePropertyTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SavePropertyType", vm);
            }

            var response = await _propertyTypeService.UpdateViewModel(vm, vm.Id);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("SavePropertyType", vm);
            }


            return RedirectToRoute(new { controller = "AdminHome", action = "PropertyTypes" });

        }

        public async Task<IActionResult> Delete(int Id)
        {
            return View(await _propertyTypeService.GetByIdViewModel(Id));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SavePropertyTypeViewModel vm)
        {

            var check = await _propertyTypeService.CheckDelete(vm.Id);
            
            if (check.HasError)
            {
                vm.HasError = check.HasError;
                vm.Error = check.Error;
                return View(vm);
            }

            var propertiesid =await _propertyTypeService.DeletePropertiesAssociatedViewModel(vm.Id);

            await _propertyTypeService.DeleteViewModel(vm.Id);

            if (propertiesid != null && propertiesid.Count > 0)
            {
                foreach (var propid in propertiesid)
                {
                    DeleteImageDirectory(propid.ToString());
                }
            }


            return RedirectToRoute(new { controller = "AdminHome", action = "PropertyTypes" });
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
