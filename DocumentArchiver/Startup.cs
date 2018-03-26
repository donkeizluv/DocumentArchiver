using System;
using System.IO;
using System.Text;
using DocumentArchiver.Helper;
using DocumentArchiver.EntityModels;
using DocumentArchiver.Helper;
using DocumentArchiver.Indus;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DocumentArchiver
{
    public class Startup
    {
        private const string SecretKey = "76ivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            EnviromentHelper.EnvStr = Configuration.GetSection("General").GetValue<string>("EnvStr");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //inject im-mem cache
            //services.AddMemoryCache();
            //Jwt
            services.AddSingleton<IJwtFactory, JwtFactory>();

            //Inject db context
            services.AddDbContext<DocumentArchiverContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("Default")));
            //Inject config
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddSingleton<IIndusAdapter>(IndusFactory.GetIndusInstance(Configuration,
                File.ReadAllText($"{Program.ExeDir}\\{Configuration.GetSection("Indus").GetValue<string>("QueryFileName")}")));

            //cookie auth service
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
            //    options =>
            //    {
            //        options.Cookie.Expiration = TimeSpan.FromMinutes(120);
            //        // access inner page w/o cred will get redirected to this
            //        options.LoginPath = new PathString("/Account/Login");
            //        options.AccessDeniedPath = new PathString("/Account/Forbidden");
            //        options.LogoutPath = new PathString("/Account/Logout");
            //        options.SlidingExpiration = true; //extend cookie exp as user still on the site
            //        //just for fun, cant find a clean way to use this :/
            //        //bc url query doesnt play well with form submit in Account/DoLogin
            //        options.ReturnUrlParameter = "returnUrl";
            //    });

            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            //policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AbilityList.Create, policy => policy.RequireClaim(AppConst.Ability, AbilityList.Create));
                options.AddPolicy(AbilityList.Delete, policy => policy.RequireClaim(AppConst.Ability, AbilityList.Delete));
                options.AddPolicy(AbilityList.Update, policy => policy.RequireClaim(AppConst.Ability, AbilityList.Update));
                options.AddPolicy(AbilityList.Download, policy => policy.RequireClaim(AppConst.Ability, AbilityList.Download));
                options.AddPolicy(AbilityList.ManageUser, policy => policy.RequireClaim(AppConst.Ability, AbilityList.ManageUser));
            });

            //Compression
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                //Everything else is too small to compress
                options.MimeTypes = new[] { "text/css", "application/javascript" };
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Fastest;
            });


            //enforce SSL
            //services.Configure<MvcOptions>(options =>
            //{
            //    options.Filters.Add(new RequireHttpsAttribute());
            //});
            //https://github.com/aspnet/Mvc/issues/4842

            services.AddSession(options =>
            {
                options.Cookie.Name = "s";
            });
            services.AddMvc().AddJsonOptions(options =>
            {
                //solve auto camel case prop names
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //ignore loop ref of object contains each other
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            //enforce SSL
            //app.UseRewriter(new RewriteOptions().AddRedirectToHttps((int)HttpStatusCode.Redirect, 44395));

            //Use this in PROD
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseBrowserLink();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            app.UseResponseCompression();
            app.UseDeveloperExceptionPage();
            app.UseBrowserLink();
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
                routes.MapRoute(
                   name: "api",
                   template: "API/{controller}/{action}");
                //Use this to fallback route in case of using vue router heavily
                //Install - Package Microsoft.AspNetCore.SpaServices
                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }

    }
}
