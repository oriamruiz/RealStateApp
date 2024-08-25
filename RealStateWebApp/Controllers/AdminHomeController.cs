using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces.Services;

namespace RealStateApp.WebApp.Controllers
{
    [Authorize(Roles ="ADMIN")]
    public class AdminHomeController : Controller
    {
        private readonly IAdminHomeService _adminHomeService;

        public AdminHomeController(IAdminHomeService adminHomeService)
        {
            _adminHomeService = adminHomeService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _adminHomeService.GetAdminHomeAsync());
        }

        public async Task<IActionResult> Agents()
        {
            return View(await _adminHomeService.GetAgentsAdminHomeAsync());
        }

        public async Task<IActionResult> Admins()
        {
            return View(await _adminHomeService.GetAdminsAdminHomeAsync());
        }

        public async Task<IActionResult> Developers()
        {
            return View(await _adminHomeService.GetDevelopersAdminHomeAsync());
        }

        public async Task<IActionResult> PropertyTypes()
        {
            return View(await _adminHomeService.GetPropertyTypesAdminHomeAsync());
        }

        public async Task<IActionResult> SaleTypes()
        {
            return View(await _adminHomeService.GetSaleTypesAdminHomeAsync());
        }

        public async Task<IActionResult> Improvements()
        {
            return View(await _adminHomeService.GetImprovementssAdminHomeAsync());
        }

    }
}
