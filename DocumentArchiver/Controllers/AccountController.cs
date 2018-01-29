﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DocumentArchiver.EntityModels;
using DocumentArchiver.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DocumentArchiver.Controllers
{
    public class AccountController : Controller
    {
        public static readonly string LoginStatusKey = "LoginStatus";
        private DocumentArchiverContext _context;
        private IConfiguration _config;

        internal string Issuer
        {
            get
            {
                return _config.GetSection("Authentication").GetValue<string>("Issuer");
            }
        }
        internal bool NoPwdCheck
        {
            get
            {
                return _config.GetSection("Authentication").GetValue<bool>("NoPwdCheck");
            }
        }

        internal string Domain
        {
            get
            {
                return _config.GetSection("Authentication").GetValue<string>("Domain");
            }
        }

        public AccountController(DocumentArchiverContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public enum LoginResult
        {
            Error,
            NotActive,
            NoPermission,
            OK
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            HttpContext.Response.Headers.Add("Login", EnviromentHelper.LoginUrl.ToString());
            if (User.Identities.Any(u => u.IsAuthenticated))
            {
                return RedirectToAction("Index", "Home");
            }
            if (TempData.ContainsKey(LoginStatusKey))
                ViewBag.LoginStatus = TempData[LoginStatusKey];
            //return login form view
            ViewBag.NoFooter = true;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DoLogin([FromForm]string userName = "", [FromForm]string pwd = "")
        {
            using (_context)
            {
                //remove this incase of recording user last login
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                //clear whatever stored session
                ClearSession();
                var loginLevel = GetLoginLevel(userName, pwd, _context, out var user);
                if (loginLevel == LoginResult.Error) return LoginFail();
                if (loginLevel == LoginResult.NoPermission) return NoPermission();
                if (loginLevel == LoginResult.NotActive) return NotActive();
                //OK proceed
                //claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userName.ToLower(), ClaimValueTypes.String, Issuer),
                    new Claim(ClaimTypes.Role, loginLevel.ToString(), ClaimValueTypes.String, Issuer)
                };
                //add abilities to claims 
                foreach (var ability in user.UserAbility)
                {
                    claims.Add(new Claim(ClaimTypes.Role, ability.AbilityName));
                }
                //add claims to identity
                var userIdentity = new ClaimsIdentity("UserCred");
                userIdentity.AddClaims(claims);
                //add identity to principal
                var userPrincipal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
                        //IsPersistent = false,
                        //AllowRefresh = false
                    });
                return RedirectToAction("Index", "Home");
            }
        }

        private IActionResult LoginFail()
        {
            TempData[LoginStatusKey] = "Đăng nhập thất bại."; //pass data to redirect
            return RedirectToAction("Login", "Account");
        }
        private IActionResult NoPermission()
        {
            TempData[LoginStatusKey] = "Không tìm thấy quyền truy cập."; //pass data to redirect
            return RedirectToAction("Login", "Account");
        }
        private IActionResult NotActive()
        {
            TempData[LoginStatusKey] = "Tài khoản đã bị vô hiệu hóa."; //pass data to redirect
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            ClearSession();
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Forbidden()
        {
            return View();
        }

        private void ClearSession()
        {
            HttpContext.Session.Clear();
        }
        private bool ValidateCredentials(string userName, string pwd)
        {
            if (NoPwdCheck) return true;
            using (var pc = new System.DirectoryServices.AccountManagement
                .PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain, Domain))
            {
                // validate the credentials
                return pc.ValidateCredentials(userName, pwd);
            }
        }
        private LoginResult GetLoginLevel(string userName, string pwd, DocumentArchiverContext context, out User user)
        {
            user = null;
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(pwd))
                return LoginResult.Error;
            if (!ValidateCredentials(userName, pwd)) return LoginResult.Error;
            user = context.User.Include(u => u.UserAbility).FirstOrDefault(u => u.Username == userName);
            if (user == null)
                return LoginResult.NoPermission; //no permission
            if (!user.Active)
                return LoginResult.NotActive;
            return LoginResult.OK;
        }
    }
}