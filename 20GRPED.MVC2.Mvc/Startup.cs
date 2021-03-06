﻿using _20GRPED.MVC2.Crosscutting.Identity;
using _20GRPED.MVC2.Domain.Model.Interfaces.Services;
using _20GRPED.MVC2.Domain.Model.Options;
using _20GRPED.MVC2.InversionOfControl;
using _20GRPED.MVC2.Mvc.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace _20GRPED.MVC2.Mvc
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
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddRazorPages(); //Auth

            services.RegisterInjections(Configuration);
            services.RegisterConfigurations(Configuration);
            services.RegisterIdentity(Configuration);

            services.AddAuthorization(
                options => options.AddPolicy("Admin", policy => policy.RequireClaim("AdminClaim")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage(); //Auth - nuget Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); //Auth
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages(); //Auth
            });
        }
    }
}
