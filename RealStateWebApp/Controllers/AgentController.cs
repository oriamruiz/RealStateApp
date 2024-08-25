using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Helpers.Sessions;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels.Agents;
using RealStateApp.Core.Application.ViewModels.Properties;
using RealStateApp.Core.Application.ViewModels.Users;

namespace RealStateApp.WebApp.Controllers
{
    

    public class AgentController : Controller
    {
        private readonly IUserService _userService;

        public AgentController(IUserService userService)
        {
            _userService = userService;
        }



        [Authorize(Roles = "AGENTE")]
        public async Task<IActionResult> Update()
        {
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            return View(await _userService.GetByIdUpdateAgentViewModelAsync(user.Id));

        }

        [Authorize(Roles = "AGENTE")]
        [HttpPost]
        public async Task<IActionResult> Update(UpdateAgentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);

            }

            var response = await _userService.UpdateAgentAsync(vm);

            if(response.HasError) 
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }

            vm.AccountImgUrl = UploadFile(vm.ImageFile, vm.Id, vm.AccountImgUrl);
            
            await _userService.UpdateImgViewModel(new Core.Application.ViewModels.Users.UpdateImgViewModel { Id = vm.Id, AccountImgUrl = vm.AccountImgUrl });


            return RedirectToRoute(new { controller = "AgentHome", action = "Index", Id = vm.Id });

        }

        [Authorize(Roles = "ADMIN")]
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
            
            return RedirectToRoute(new { controller = "AdminHome", action = "Agents" });
        }


        [Authorize(Roles = "ADMIN")]

        public async Task<IActionResult> Delete(string Id)
        {
            return View(new DeleteAgentViewModel { Id = Id });
        }

        [Authorize(Roles = "ADMIN")]

        [HttpPost]
        public async Task<IActionResult> DeletePost(DeleteAgentViewModel vm)
        {
            vm = await _userService.DeleteAgentAsync(vm.Id);

            if (vm.HasError)
            {
                return View("Delete", vm);
            }

            DeleteGenericImageDirectory(vm.Id, "Users");

            if (vm.Properties != null &&  vm.Properties.Count > 0)
            {
                foreach (var propid in vm.Properties)
                {
                    DeleteGenericImageDirectory(propid.ToString(), "Properties");
                }
            }
            
            return RedirectToRoute(new { controller = "AdminHome", action = "Agents" });
        }




        #region private methods
        private string UploadFile(IFormFile file, string id, string ImageUrl = "")
        {

            if (file == null)
            {
                return ImageUrl;
            }

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

            string[] oldimgPart = ImageUrl.Split("/");
            string oldimagename = oldimgPart[^1];
            string completeImageOldPath = Path.Combine(path, oldimagename);

            if (System.IO.File.Exists(completeImageOldPath))
            {
                System.IO.File.Delete(completeImageOldPath);
            }

            return $"{basepath}/{filename}";

        }

        private void DeleteGenericImageDirectory(string id, string Directorytodelete)
        {

            string basepath = $"/images/{Directorytodelete}/{id}";
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
