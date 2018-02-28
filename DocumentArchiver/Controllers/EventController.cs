using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using DocumentArchiver.EntityModels;
using DocumentArchiver.Filter;
using DocumentArchiver.Helper;
using DocumentArchiver.ViewModels;
using DocumentArchiver.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;
using static DocumentArchiver.ApiParameter.ListingParams;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DocumentArchiver.Controllers
{
    [Authorize]
    [CustomExceptionFilterAttribute]
    public class EventController : Controller
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public IEnumerable<string> AcceptFormats
        {
            get
            {
                //var value = _config.GetSection("FileStorage").GetValue<List<string>>(nameof(AcceptFormats));
                var values = _config.GetSection("FileStorage").GetSection(nameof(AcceptFormats)).AsEnumerable().Select(k => k.Value);
                if (values == null || values.Count() < 1)
                    //default if not set
                    return new List<string>() { ".jpg", ".jpeg", ".bmp", ".png", ".doc", ".docx", ".pdf", ".msg" }; 
                return values;
            }
        }
        public int MaxFileSize
        {
            get
            {
                var value = _config.GetSection("FileStorage").GetValue<int>(nameof(MaxFileSize));
                if (value == default(int))
                    return 3145728;
                return value;
            }
        }
        public string PathUploadPath
        {
            get
            {
                var value = _config.GetSection("FileStorage").GetValue<string>(nameof(PathUploadPath));
                if (string.IsNullOrEmpty(value))
                    return "Uploaded"; //default if not set
                return value;
            }
        }

        private DocumentArchiverContext _context;
        private IConfiguration _config;

        public EventController(DocumentArchiverContext context, IConfiguration config)
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
                var paramBuilder = new ParamBuilder()
                    .SetId(id)
                    .SetPage(page)
                    .SetOrderBy(order)
                    .SetAsc(asc)
                    .SetHttpContext(HttpContext)
                    .SetDbContext(_context);
                var model = await factory.Create(paramBuilder.Build());
                return Ok(model);
            }
        }

        //Download individual file
        [HttpGet]
        [Authorize(Policy = nameof(AbilityList.Download))]
        public async Task<IActionResult> Download([FromQuery]string id)
        {
            if (!Utility.Decode64(id, out int eventId)) return BadRequest();

            using (_context)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var eventLog = await _context.EventLog.SingleOrDefaultAsync(e => e.EventId == eventId);
                if (eventLog == null) return BadRequest("Cant find EventLog");
                if(!FileStorage.GetUpload(eventLog.Guid, Path.Combine(Program.ExeDir, PathUploadPath), out FileStream stream))
                {
                    return NoContent();
                }
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, eventLog.Filename);
            }
        }
        [HttpGet]
        [Authorize(Policy = nameof(AbilityList.Download))]
        //Download all files of case
        public async Task<IActionResult> DownloadZip([FromQuery]string id)
        {
            if (!Utility.Decode64(id, out int contractId)) return BadRequest();

            using (_context)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var q = _context.Contract.Where(c => c.ContractId == contractId).Include(c => c.EventLog);
                var contract = await q.SingleOrDefaultAsync();
                if (contract == null) return BadRequest("Cant find Contrcat");

                var files = contract.EventLog.Select(e => new KeyValuePair<string, string>(e.Guid, $"{e.EventId}_{e.Filename}"));
                var stream = new MemoryStream();
                var archive = new ZipArchive(stream, ZipArchiveMode.Create, true);

                foreach (var file in files)
                {
                    var fileName = file.Value;
                    if (!FileStorage.GetUpload(file.Key, Path.Combine(Program.ExeDir, PathUploadPath), out byte[] bytes))
                    {
                        _logger.Error($"Cant find {file.Key} to zip");
                        return NoContent();
                    }
                    var zipArchiveEntry = archive.CreateEntry(fileName, CompressionLevel.Fastest);
                    using (var zipStream = zipArchiveEntry.Open())
                    {
                        zipStream.Write(bytes, 0, bytes.Length);
                        bytes = null;
                    }
                }
                stream.Position = 0;
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Zip,
                    $"{contract.ContractNumber}_{DateTime.Today.ToString(AppConst.OnlyNumberDate)}.zip");
            }
        }

        [HttpPost]
        [Authorize(Policy = nameof(AbilityList.Create))]
        public async Task<IActionResult> Create([FromForm] NewEventLogPost post)
        {
            var file = post.File;
            if (!CheckUploadFileValid(file, out string extention, out string checkReason))
            {
                return BadRequest(checkReason);
            }
            if (string.IsNullOrEmpty(post.Name))
            {
                return BadRequest($"Invalid values: {nameof(NewEventLogPost.Name)}");
            }
            using (_context)
            {
                var contract = await _context.Contract.SingleOrDefaultAsync(c => c.ContractId == post.ContractId);
                if (contract == null) return BadRequest("Cant find contract id");
                var fileGuid = Guid.NewGuid().ToString();
                var eventLog = new EventLog()
                {
                    Name = post.Name,
                    DateOfEvent = post.DateOfEvent,
                    ContractId = post.ContractId,
                    CreateTime = DateTime.Now,
                    Filetype = extention,
                    Filename = file.FileName,
                    Guid = fileGuid,
                    //Replace empty or JSON null with const
                    Note = (post.Note?? AppConst.NA) == AppConst.JSONNull? AppConst.NA : post.Note,
                    Username = Utility.GetContextUsername(HttpContext)
                };
                await FileStorage.SaveUpload(file, fileGuid, Path.Combine(Program.ExeDir, PathUploadPath));
                _context.EventLog.Add(eventLog);
                await _context.SaveChangesAsync();
                return Ok();
            }
        }
        [HttpPost]
        [Authorize(Policy = nameof(AbilityList.Update))]
        public async Task<IActionResult> Update([FromForm] UpdateEventLogPost post)
        {
            if (string.IsNullOrEmpty(post.Name))
            {
                return BadRequest($"Invalid values: {nameof(NewEventLogPost.Name)}");
            }
            using (_context)
            {
                var eventLog = await _context.EventLog.SingleOrDefaultAsync(e => e.EventId == post.EventId);
                if (eventLog == null) return BadRequest("Cant find EventLog");

                eventLog.Name = post.Name;
                eventLog.Note = post.Note;
                eventLog.DateOfEvent = post.DateOfEvent;
                await _context.SaveChangesAsync();
                return Ok();
            }
        }
        [HttpPost]
        [Authorize(Policy = nameof(AbilityList.Delete))]
        public async Task<IActionResult> Delete([FromForm]int eventId)
        {
            using (_context)
            {
                //TODO: Log this action
                var eventLog = await _context.EventLog.SingleOrDefaultAsync(e => e.EventId == eventId);
                if (eventLog == null) return BadRequest("Cant find EventLog");
                _context.EventLog.Remove(eventLog);
                await _context.SaveChangesAsync();
                FileStorage.DeleteUpload(eventLog.Guid, Path.Combine(Program.ExeDir, PathUploadPath));
                return Ok();
            }
        }
        private bool CheckUploadFileValid(IFormFile file, out string extention, out string reason)
        {
            reason = string.Empty;
            extention = Path.GetExtension(file.FileName).ToLower();
            if (!AcceptFormats.Contains(extention))
            {
                reason = "Invalid extention";
                return false;
            }

            if (file.Length > MaxFileSize)
            {
                reason = "Size too big";
                return false;
            }
            return true;
        }
    }
}
