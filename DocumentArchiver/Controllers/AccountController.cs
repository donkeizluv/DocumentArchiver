﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DocumentArchiver.Helper;
using DocumentArchiver.EntityModels;
using DocumentArchiver.Filter;
using DocumentArchiver.Helper;
using DocumentArchiver.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using DocumentArchiver.Auth;

namespace DocumentArchiver.Controllers
{
    [CustomExceptionFilterAttribute]
    public class AccountController : Controller
    {
        //public const string AuthInterop = "Interop";
        public const string AuthPrincipal = "Principal";
        public const string AuthPrincipal2 = "Principal2";

        private DocumentArchiverContext _context;
        private IConfiguration _config;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;

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
        internal string AuthMethod
        {
            get
            {
                var method = _config.GetSection("Authentication").GetValue<string>("Method");
                if (string.IsNullOrEmpty(method))
                    return AuthPrincipal;
                return method;
            }
        }

        internal string Domain
        {
            get
            {
                return _config.GetSection("Authentication").GetValue<string>("Domain");
            }
        }

        public AccountController(DocumentArchiverContext context,
            IConfiguration config,
            IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions)
        {
            _context = context;
            _config = config;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
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
            //Redirects if already authenticated
            if (User.Identities.Any(u => u.IsAuthenticated))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.NoFooter = true;
            return View();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Info()
        {
            var model = new UserInfoViewModel() { Claims = SessionHelper.ClaimsToDict(HttpContext.User.Claims) };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DoLogin([FromForm]string username = "", [FromForm]string pwd = "")
        {
            using (_context)
            {
                //clear whatever stored session
                ClearSession();
                var loginLevel = GetLoginLevel(username, pwd, _context, out var user);
                if (loginLevel == LoginResult.Error) return Unauthorized();
                if (loginLevel == LoginResult.NoPermission) return Unauthorized();
                if (loginLevel == LoginResult.NotActive) return Unauthorized();
                //OK proceed
                //add claims to identity
                var userIdentity = new ClaimsIdentity("UserCred");
                userIdentity.AddClaims(GetUserClaims(user));
                var jwt = await Token.GenerateJwt(userIdentity,
                    _jwtFactory,
                    user.Username,
                    _jwtOptions,
                    new JsonSerializerSettings { Formatting = Formatting.Indented });
                await _context.SaveChangesAsync();
                //Return token
                return new OkObjectResult(jwt);

                //add identity to principal
                //var userPrincipal = new ClaimsPrincipal(userIdentity);
                //await HttpContext.SignInAsync(
                //    CookieAuthenticationDefaults.AuthenticationScheme,
                //    userPrincipal,
                //    new AuthenticationProperties
                //    {
                //        ExpiresUtc = DateTime.UtcNow.AddMinutes(45)
                //        //IsPersistent = false,
                //        //AllowRefresh = false
                //    });
                //Save lastlogin
                //await _context.SaveChangesAsync();
                //return Ok(new { Valid = true, Message = string.Empty });
            }
        }
        private List<Claim> GetUserClaims(User user)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username.ToLower()),
                    new Claim(AppConst.LayerName, user.LayerName),
                    new Claim(AppConst.LayerRank, user.LayerNameNavigation.Rank.ToString())
                };
            //add abilities to claims 
            foreach (var ability in user.UserAbility)
            {
                claims.Add(new Claim(AppConst.Ability, ability.AbilityName));
            }
            return claims;
        }
        //[HttpGet]
        //[Authorize]
        //public async Task<IActionResult> LogOut()
        //{
        //    ClearSession();
        //    await HttpContext.SignOutAsync();
        //    return RedirectToAction("Login", "Account");
        //}

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
        private bool Validate(string userName, string pwd)
        {
            if (NoPwdCheck) return true;

            switch (AuthMethod)
            {
                //case AuthInterop:
                //    return WindowsAuth.Validate_Interop(userName, pwd, Domain);
                case AuthPrincipal:
                    return WindowsAuth.Validate_Principal(userName, pwd, Domain);
                case AuthPrincipal2:
                    return WindowsAuth.Validate_Principal2(userName, pwd, Domain);
                default:
                    throw new InvalidOperationException($"Auth method: {AuthPrincipal} is not valid");
            }
        }
        private LoginResult GetLoginLevel(string userName, string pwd, DocumentArchiverContext context, out User user)
        {
            user = null;
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(pwd))
                return LoginResult.Error;
            if (!Validate(userName, pwd)) return LoginResult.Error;
            //Includes everything needs to be added to Claims
            user = context.User.Include(u => u.UserAbility)
                .Include(u => u.LayerNameNavigation)
                .FirstOrDefault(u => u.Username == userName);
            if (user == null)
                return LoginResult.NoPermission; //no permission
            if (!user.Active)
                return LoginResult.NotActive;

            user.LastLogin = DateTime.Today;
            return LoginResult.OK;
        }
    }
}
