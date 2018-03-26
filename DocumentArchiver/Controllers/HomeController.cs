using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DocumentArchiver.Models;
using Microsoft.AspNetCore.Authorization;
using DocumentArchiver.EntityModels;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DocumentArchiver.ViewModels;
using DocumentArchiver.Filter;
using static DocumentArchiver.ApiParameter.ListingParams;
using DocumentArchiver.Helper;

namespace DocumentArchiver.Controllers
{
    [Authorize]
    [CustomExceptionFilterAttribute]
    public class HomeController : Controller
    {
        private DocumentArchiverContext _context;
        private IConfiguration _config;
        public HomeController(DocumentArchiverContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
