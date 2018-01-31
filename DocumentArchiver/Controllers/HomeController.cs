using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DocumentArchiver.Models;
using Microsoft.AspNetCore.Authorization;
using DocumentArchiver.EntityModels;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DocumentArchiver.ViewModels;

namespace DocumentArchiver.Controllers
{
    [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Index([FromQuery]int page = 1, [FromQuery]string type = "", [FromQuery]string contain = "", [FromQuery]string order = "", [FromQuery]bool asc = true)
        {
            using (_context)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var model = await ModelFactory.CreateCaseListingModel(_context, page, type, contain, order, asc);
                return View(model);
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
