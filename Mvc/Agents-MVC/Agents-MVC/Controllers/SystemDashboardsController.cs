using Agents_MVC.Service;
using Microsoft.AspNetCore.Mvc;

namespace Agents_MVC.Controllers
{
    public class SystemDashboardsController(ISystemDashboardService dashboardService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var dashboard = await dashboardService.AddGeneralDashboardVM();
            return View(dashboard);
        }

        public async Task<IActionResult> AgentsDetails()
        {
            var agentsDetailsList = await dashboardService.AddAgentsDetails();
            return View(agentsDetailsList);
        }

        public async Task<IActionResult> TargetsDetails()
        {
            var targetsDetailsList = await dashboardService.AddTargetsDetails();
            return View(targetsDetailsList);
        }
    }
}
