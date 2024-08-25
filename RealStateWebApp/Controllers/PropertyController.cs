using InternetBankingApp.Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Helpers.Sessions;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.MiddleWare;

namespace RealStateApp.WebApp.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        

        [Authorize(Roles = "AGENTE")]

        public async Task<IActionResult> Create()
        {
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            return View("SaveProperty", await _propertyService.GetSavePropertyViewModelView(new SavePropertyViewModel { AgentId = user.Id }));
        }

        [Authorize(Roles = "AGENTE")]

        [HttpPost]
        public async Task<IActionResult> Create(SavePropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm = await _propertyService.GetSavePropertyViewModelView(vm);
                return View("SaveProperty", vm);
            }

            vm = await _propertyService.CreateViewModel(vm);

            if(vm ==null || vm.HasError)
            {
                return View("SaveProperty", vm);
            }

            vm.MainImageUrl = UploadMainImageFile(vm.MainImageFile, vm.Id);
            (vm.ImageUrl2, vm.ImageUrl3, vm.ImageUrl4) = AssignUrlsToImages(vm.optionalImagesFile, vm.Id, vm.ImageUrl2, vm.ImageUrl3, vm.ImageUrl4);

            await _propertyService.UpdateImgViewModel(vm, vm.Id);

            return RedirectToRoute(new { controller = "AgentHome", action = "GetAllByAgentId"});
        }

        [Authorize(Roles = "AGENTE")]
        public async Task<IActionResult> Update(int id)
        {
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            return View("SaveProperty", await _propertyService.GetSavePropertyViewModelView(new SavePropertyViewModel { AgentId = user.Id, Id = id}));
        }

        [Authorize(Roles = "AGENTE")]
        [HttpPost]
        public async Task<IActionResult> Update(SavePropertyViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                vm = await _propertyService.GetSavePropertyViewModelView(vm);
                return View("SaveProperty", vm);
            }

            var vmupdated = await _propertyService.UpdateViewModel(vm, vm.Id);

            if (vmupdated.HasError) 
            {
                vm = await _propertyService.GetSavePropertyViewModelView(vm);
                vm.HasError = vmupdated.HasError;
                vm.Error = vmupdated.Error;
                return View("SaveProperty", vm);
            }

            vm.MainImageUrl = UploadMainImageFile(vm.MainImageFile, vm.Id, vm.MainImageUrl,true);


            if(vm.optionalImagesFile != null && vm.optionalImagesFile.Count > 0)
            {
                CleanOptionalImagesDirectory(vm.Id);
                (vm.ImageUrl2, vm.ImageUrl3, vm.ImageUrl4) = AssignUrlsToImages(vm.optionalImagesFile, vm.Id, vm.ImageUrl2, vm.ImageUrl3, vm.ImageUrl4);
            }

            await _propertyService.UpdateImgViewModel(vm, vm.Id);


            return RedirectToRoute(new { controller = "AgentHome", action = "GetAllByAgentId" });
        }


        [Authorize(Roles = "AGENTE")]
        public async Task<IActionResult> Delete(int Id)
        {
            return View(new DeletePropertyViewModel { Id = Id});
        }


        [Authorize(Roles = "AGENTE")]
        [HttpPost]
        public async Task<IActionResult> DeletePost(DeletePropertyViewModel vm)
        {
            var result = await _propertyService.CheckDelete(vm.Id);

            if(result.HasError )
            {
                vm.HasError = result.HasError;
                vm.Error = result.Error;
                return View("Delete", vm);
            }
            
            await _propertyService.DeleteViewModel(vm.Id);
            DeletePropertyImageDirectory(vm.Id);

            return RedirectToRoute(new { controller = "AgentHome", action = "GetAllByAgentId" });
        }



        #region private methods

        private string UploadMainImageFile(IFormFile file, int id, string ImageUrl = "", bool isEditMode =false)
        {

            if (isEditMode && file == null)
            {
                return ImageUrl;
            }

            string basepath = $"/images/Properties/{id}/MainImage";

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

            if(isEditMode)
            {
                string[] oldimgPart = ImageUrl.Split("/");
                string oldimagename = oldimgPart[^1];
                string completeImageOldPath = Path.Combine(path, oldimagename);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }
            
            return $"{basepath}/{filename}";

        }

        private string UploadOptionalImageFile(IFormFile file, int id, string ImageUrl ="")
        {
            
            if(file == null)
            {
                return ImageUrl = null;
            }

            string basepath = $"/images/Properties/{id}/OptionalImages";

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

        private (string, string, string) AssignUrlsToImages(List<IFormFile> files, int Id ,string url1, string url2, string url3)
        {

            if(files.Count>0)
            {
                url1 = UploadOptionalImageFile(files[0], Id, url1);

            }
            else
            {
                url1 = UploadOptionalImageFile(null, Id, url1);

            }

            if (files.Count > 1)
            {
                url2 = UploadOptionalImageFile(files[1], Id, url2);
            }
            else
            {
                url2 = UploadOptionalImageFile(null, Id, url2);
            }

            if (files.Count > 2)
            {
                url3 = UploadOptionalImageFile(files[2], Id, url3);
            }
            else
            {
                url3 = UploadOptionalImageFile(null, Id, url3);
            }


            return (url1, url2, url3);
        }


        private void CleanOptionalImagesDirectory(int propertyId)
        {
            
            string basepath = $"/images/Properties/{propertyId}/OptionalImages";
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

            }
        }

        private void DeletePropertyImageDirectory(int id)
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

