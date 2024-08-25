using Agents_MVC.Service;
using Microsoft.AspNetCore.Mvc;

namespace Agents_MVC.Controllers
{
    public class MissionsManagementController(IMissionsManagementService managementService) : Controller
    {
        
        public async Task<IActionResult> Index()
        {
            var allOffers = await managementService.GetAllOffers();
            return View(allOffers);
        }
    }
}
