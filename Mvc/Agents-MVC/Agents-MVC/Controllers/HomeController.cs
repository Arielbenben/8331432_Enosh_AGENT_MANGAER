using Agents_MVC.Models;
using Agents_MVC.Service;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Agents_MVC.Controllers
{
    public class HomeController(IMatrixService matrixService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await matrixService.InitMatrix());
        }
    }
}
