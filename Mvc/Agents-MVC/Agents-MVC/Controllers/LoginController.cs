using Microsoft.AspNetCore.Mvc;

namespace Agents_MVC.Controllers
{
    public class LoginController : Controller
    {

        public IActionResult Login()
        {
            return View();
        }

    }
}
