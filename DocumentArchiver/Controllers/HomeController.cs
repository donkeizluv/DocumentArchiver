using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DocumentArchiver.Models;
using Microsoft.AspNetCore.Authorization;

namespace DocumentArchiver.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
