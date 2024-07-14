using GleamTech.AspNet.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using WebRaport.Authorization;
using WebRaport.Interfaces;
using WebRaport.Repository;

namespace WebRaport
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGleamTech();
            services.AddTransient<IUserRepository, MocUsersRepository>();
            services.AddTransient<IRaportRepository, MocRaportRepository>();
            services.AddTransient<IPermissionRepository, MocPermissionRepository>();
            services.AddTransient<IFieldsRepository, MocFieldsRepository>();
            services.AddSingleton<IAuthorizationHandler, AuthHandler>();
            services.AddAuthorization(options =>
                options.AddPolicy("AdminRequiredPermission", policy => policy.
                AddRequirements(new AuthRequired(new string[] { "admin" }))));
            services.AddAuthorization(options =>
                options.AddPolicy("EditorRequiredPermission", policy => policy.
                AddRequirements(new AuthRequired(new string[] { "editor", "admin" }))));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = new PathString("/Login/Login");
                options.AccessDeniedPath = new PathString("/Login/AccessDenied");
            });
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddRazorPages();
            //подключение Newtonsoft Json с PascalCase по дефолту
            services.AddControllers().AddNewtonsoftJson(op => op.SerializerSettings.ContractResolver = new DefaultContractResolver()); 
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseGleamTech();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
