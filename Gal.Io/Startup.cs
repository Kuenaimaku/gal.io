using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gal.Io.Interfaces;
using Gal.Io.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gal.Io
{
    public class Startup
    {
        private readonly ILogger _logger;
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddResponseCaching();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Serve up configuration file via DI for easy use
            services.AddSingleton(Configuration);
            //Dependency Injection - add services here
            _logger.LogInformation("Adding RiotService");
            services.AddTransient<IRiotService, RiotService>();
            services.AddTransient<IPlayerService, PlayerService>();
            services.AddSingleton<IChampionService, ChampionService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder =>
                builder.WithOrigins(Configuration.GetSection("Riot").GetValue<string>("BaseUri"))
                .WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader()
                .WithOrigins("http://ddragon.leagueoflegends.com/"));

            app.UseResponseCaching();
            app.Use(async (context, next) =>
            {
                // For GetTypedHeaders, add: using Microsoft.AspNetCore.Http;
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(10)
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                await next();
            });

            app.UseMvc();
            //Initialize InDesignService on Startup instead of first invocation via API
            app.ApplicationServices.GetService<IChampionService>();
        }
    }
}
