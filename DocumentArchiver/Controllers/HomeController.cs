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
        [Authorize]
        public async Task<IActionResult> Index([FromQuery]int page = 1,
            [FromQuery]string type = "",
            [FromQuery]string contain = "",
            [FromQuery]string order = "",
            [FromQuery]bool asc = true)
        {
            using (_context)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var factory = new ContractVMFactory(_context);
                var paramBuilder = new ParamBuilder()
                   .SetPage(page)
                   .SetType(type).SetContain(contain)
                   .SetOrderBy(order)
                   .SetAsc(asc)
                   .SetHttpContext(HttpContext)
                   .SetDbContext(_context);
                var model = await factory.Create(paramBuilder.Build());
                //Fill claims to init app permission
                model.Claims = SessionHelper.ClaimsToDict(HttpContext.User.Claims);
                return View(model);
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
