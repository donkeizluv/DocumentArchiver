using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentArchiver.Filter;
using DocumentArchiver.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DocumentArchiver.Controllers
{
    [Authorize(Policy = nameof(AbilityList.ManageUser))]
    [CustomExceptionFilterAttribute]
    public class AdmController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create()
        {
            return Ok(new { Result = "OK", Bla = "bla" });
        }

        [HttpPost]
        public IActionResult Update()
        {
            return Ok(new { Result = "OK", Bla = "bla" });
        }

        [HttpPost]
        public IActionResult Delete()
        {
            return Ok(new { Result = "OK", Bla = "bla" });
        }
    }
}
