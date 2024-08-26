using Agents_MVC.Service;
using Agents_MVC.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Agents_MVC.Controllers
{
    public class MissionsManagementController(IMissionsManagementService managementService) : Controller
    {
        
        public async Task<IActionResult> Index()
        {
            var allOffers = await managementService.CreateAllMissionsVm();
            return View(allOffers);
        }

        public async Task<IActionResult> Instruct(int id)
        {
            await managementService.InstructMission(id);
            return View();
        }
    }
}
