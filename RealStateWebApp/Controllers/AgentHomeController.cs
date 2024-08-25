using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Helpers.Sessions;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels.Properties;

namespace RealStateApp.WebApp.Controllers
{
    [Authorize(Roles = "AGENTE")]

    public class AgentHomeController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IUserService _userService;
        private readonly IPropertyTypeService _propertyTypeService;

        public AgentHomeController(IPropertyService propertyService, IUserService userService, IPropertyTypeService typeService)
        {
            _propertyService = propertyService;
            _userService = userService;
            _propertyTypeService = typeService;
        }


        public async Task<IActionResult> Index()
        {
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            return View(await _propertyService.GetAllByAgentIdViewModel(user.Id));
        }

        public async Task<IActionResult> Details(int id, string RedirectTo)
        {
            var vm = await _propertyService.GetByIdDetailsViewModel(id);
            vm.RedirectTo = RedirectTo;
            return View(vm);

        }

        public async Task<IActionResult> GetAllByAgentId()
        {
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            return View(await _propertyService.GetAllByAgentIdViewModel(user.Id));
        }


        [HttpPost]
        public async Task<IActionResult> SearchAgentPropertyByCode(string Code, string RedirectTo)
        {
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            return View(RedirectTo, await _propertyService.GetAgentPropertiesByCodeViewModel(Code, user.Id));

        }

        [HttpPost]
        public async Task<IActionResult> AgentPropertyFilters(FiltersPropertyViewModel vm, string RedirectTo)
        {
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            return View(RedirectTo, await _propertyService.GetAgentPropertiesByFiltersViewModel(vm, user.Id));

        }
    }
}
