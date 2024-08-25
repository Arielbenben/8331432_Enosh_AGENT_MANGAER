using Agents_MVC.Service;
using Microsoft.AspNetCore.Mvc;

namespace Agents_MVC.Controllers
{
    public class MissionsDetailsController(IMissionDetailsService detailsService) : Controller
    {
        public async Task<IActionResult> Index(int id)
        {
            var mission = await detailsService.GetMissionById(id);
            return View(mission);
        }
    }
}
