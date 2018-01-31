using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentArchiver.EntityModels;
using DocumentArchiver.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DocumentArchiver.Controllers
{
    [Route("API/EventListing/[action]")]
    [Authorize]
    public class EventListingController : Controller
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private DocumentArchiverContext _context;
        private IConfiguration _config;

        public EventListingController(DocumentArchiverContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int id, [FromQuery]int page = 1, [FromQuery]string order = "", [FromQuery]bool asc = true)
        {
            using (_context)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var factory = new EventVMFactory(_context);
                var model = await factory.Create(id, page, order, asc);
                return Ok(model);
            }
        }
    }
}
