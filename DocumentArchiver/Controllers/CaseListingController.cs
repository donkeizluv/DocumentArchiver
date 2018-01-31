using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DocumentArchiver.ViewModels;
using DocumentArchiver.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;

namespace DocumentArchiver.Controllers
{
    [Route("API/ItemListing/[action]")]
    [Authorize]
    public class ItemListingController : Controller
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private DocumentArchiverContext _context;
        private IConfiguration _config;

        public ItemListingController(DocumentArchiverContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetCaseListingModel([FromQuery]int page = 1, [FromQuery]string type = "", [FromQuery]string contain = "", [FromQuery]string order = "", [FromQuery]bool asc = true)
        {
            using (_context)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var model = await ModelFactory.CreateCaseListingModel(_context, page, type, contain, order, asc);
                return Ok(model);
            }
        }

       
    }
}
