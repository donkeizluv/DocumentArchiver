using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DocumentArchiver.ViewModels;
using DocumentArchiver.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;
using DocumentArchiver.Wrapper;
using DocumentArchiver.Indus;
using DocumentArchiver.Helper;
using System.Linq;

namespace DocumentArchiver.Controllers
{
    [Authorize]
    public class ContractController : Controller
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private DocumentArchiverContext _context;
        private IConfiguration _config;
        private IIndusAdapter _indus;

        public ContractController(DocumentArchiverContext context, IConfiguration config, IIndusAdapter indus)
        {
            _context = context;
            _config = config;
            _indus = indus;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int page = 1, [FromQuery]string type = "", [FromQuery]string contain = "", [FromQuery]string order = "", [FromQuery]bool asc = true)
        {
            using (_context)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var factory = new ContractVMFactory(_context);
                var model = await factory.Create(page, type, contain, order, asc);
                return Ok(model);
            }
        }

        [HttpPost]
        public IActionResult Check([FromForm]string contractNumber)
        {
            if (string.IsNullOrEmpty(contractNumber)) return BadRequest();
            using (_context)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                if(CheckContract(contractNumber, out var contract, out var reason))
                {
                    return Ok(new ResponseWrapper() { Valid = true, Data = contract });
                }
                return Ok(new ResponseWrapper() { Valid = false, Message = reason });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]string contractNumber)
        {
            if (string.IsNullOrEmpty(contractNumber)) return BadRequest();
            using (_context)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                if (!CheckContract(contractNumber, out var contract, out var reason))
                {
                    return Ok(new ResponseWrapper() { Valid = false, Message = reason });
                }
                //Save to db
                contract.Username = Utility.GetContextUsername(HttpContext);
                _context.Contract.Add(contract);
                await _context.SaveChangesAsync();
                return Ok(new ResponseWrapper() { Valid = true });
            }
        }
        private bool CheckContract(string contractNumber, out Contract contract, out string reason)
        {
            reason = string.Empty;
            contract = _context.Contract.SingleOrDefault(r => r.ContractNumber == contractNumber);
            if (contract != null)
            {
                //Not valid
                reason = "Case này đã có trên hệ thống.";
                return false;
            }
            //Try get info from indus
            contract = _indus.GetContractInfo(contractNumber);
            if(contract == null)
            {
                reason = $"Không tìm thấy thông tin hợp đồng này.";
                return false;
            }
            return true;
                
        }
    }
}
