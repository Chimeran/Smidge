﻿using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Smidge;
using Smidge.Options;
using Smidge.Models;
using Smidge.FileProcessors;

namespace Smidge.Web
{
    public class Startup
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Constructor sets up the configuration - for our example we'll load in the config from appsettings.json with
        /// a sub configuration value of 'smidge'
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);            
            var config = builder.Build();
            _config = config.GetSection("smidge");
        }

        public void ConfigureServices(IServiceCollection services)
        {
			services.AddMvcCore()
				.AddJsonFormatters();

			services.AddMvc();

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();

            // Or use services.AddSmidge() to test from smidge.json config.
            services.AddSmidge(_config) 
                .Configure<SmidgeOptions>(options =>
                {
                })
                .Configure<Bundles>(bundles =>
                {
                    bundles.Create("test-bundle-1",
                        new JavaScriptFile("~/Js/Bundle1/a1.js"),
                        new JavaScriptFile("~/Js/Bundle1/a2.js"),
                        //NOTE: This is already min'd based on it's file name, therefore
                        // by convention JsMin should be removed
                        new JavaScriptFile("~/Js/Bundle1/a3.min.js"))
                        .OnOrdering(collection =>
                        {
                            //return some custom ordering
                            return collection.OrderBy(x => x.FilePath);
                        });

                    bundles.Create("test-bundle-2", WebFileType.Js, "~/Js/Bundle2");

                    bundles.Create("test-bundle-3", bundles.PipelineFactory.GetPipeline(typeof(JsMinifier)), WebFileType.Js, "~/Js/Bundle2");

                    bundles.Create("test-bundle-4",
                        new CssFile("~/Css/Bundle1/a1.css"),
                        new CssFile("~/Css/Bundle1/a2.css"));

                    bundles.Create("libs-js", WebFileType.Js, "~/Js/Libs/jquery-1.12.2.js","~/Js/Libs/knockout-es5.js");
                    bundles.Create("libs-css", WebFileType.Css, "~/Css/Libs/font-awesome.css");
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Debug);

            // Add the following to the request pipeline only in development environment.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // sends the request to the following path or controller action.
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSmidge();
        }
    }
}
