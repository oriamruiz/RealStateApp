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
    [Authorize(Roles ="CLIENTE")]
    public class ClientHomeController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IUserService _userService;
        private readonly IPropertyTypeService _propertyTypeService;

        public ClientHomeController(IPropertyService propertyService, IUserService userService, IPropertyTypeService typeService)
        {
            _propertyService = propertyService;
            _userService = userService;
            _propertyTypeService = typeService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            return View(await _propertyService.GetAllViewModel());
        }
        public async Task<IActionResult> ChangeFavoritePropertyStatus(int Id)
        {
            await _propertyService.ChangeFavoritePropertyStatusAsync(Id);
            return RedirectToRoute(new { controller = "ClientHome", action = "Index" });
        }

        
        public async Task<IActionResult> Details(int id, string RedirectTo)
        {
            var vm = await _propertyService.GetByIdDetailsViewModel(id);
            vm.RedirectTo = RedirectTo;
            return View(vm);

        }
        

        [HttpPost]
        public async Task<IActionResult> SearchByName(string Name)
        {

            return View("Agents", await _userService.GetAgentsByNameViewModel(Name));

        }

        [HttpPost]
        public async Task<IActionResult> SearchByCode(string Code)
        {
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            return View("Index", await _propertyService.GetPropertiesByCodeViewModel(Code));

        }



        [HttpPost]
        public async Task<IActionResult> Filters(FiltersPropertyViewModel vm)
        {
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            return View("Index", await _propertyService.GetPropertiesByFiltersViewModel(vm));

        }

        public async Task<IActionResult> Agents()
        {

            return View(await _userService.GetAllActiveAgentsAsync());

        }
        public async Task<IActionResult> GetAllByAgentId(string Id)
        {
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            ViewBag.AgentId = Id;
            return View(await _propertyService.GetAllByAgentIdViewModel(Id));
        }
        [HttpPost]
        public async Task<IActionResult> SearchAgentPropertyByCode(string Code, string AgentId)
        {
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            ViewBag.AgentId = AgentId;
            return View("GetAllByAgentId", await _propertyService.GetAgentPropertiesByCodeViewModel(Code, AgentId));

        }

        [HttpPost]
        public async Task<IActionResult> AgentPropertyFilters(FiltersPropertyViewModel vm, string AgentId)
        {
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            ViewBag.AgentId = AgentId;
            return View("GetAllByAgentId", await _propertyService.GetAgentPropertiesByFiltersViewModel(vm, AgentId));

        }

        public async Task<IActionResult> MyProperties()
        {
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            return View(await _propertyService.GetAllMyFavoritePropertiesViewModel());

        }

        [HttpPost]
        public async Task<IActionResult> SearchMyFavoritePropertiesByCode(string Code)
        {
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            return View("MyProperties", await _propertyService.GetMyFavoritePropertiesByCodeViewModel(Code));

        }



        [HttpPost]
        public async Task<IActionResult> MyFavoritePropertiesFilters(FiltersPropertyViewModel vm)
        {
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            return View("MyProperties", await _propertyService.GetMyFavoritePropertiesByFiltersViewModel(vm));

        }

    }
}
