using Microsoft.AspNetCore.Mvc;

namespace Agents_MVC.Controllers
{
    public class SystemDashboardsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
