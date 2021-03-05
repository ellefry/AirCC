using AirCC.Portal.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCI.Extensions.Newtonsoft;
using BCI.Extensions.Core.Json;
using Microsoft.Extensions.DependencyInjection.Extensions;
using BCI.Extensions.Core.DI;
using BCI.Extensions.EFCore;
using AirCC.Portal.EntityFramework;
using AirCC.Portal.AppService;
using BCI.Extensions.AutoMapper;
using BCI.Extensions.Core.ObjectMapping;
using AirCC.Portal.AppService.Clients;
using AirCC.Portal.Infrastructure;
using AirCC.Portal.WebServers;

namespace AirCC.Portal
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
            services.AddHttpContextAccessor();
            services.AddTypes().AddOptions(Configuration, true);
            services.TryAddSingleton<IEntityMappingManager,EntityMappingManager>();
            services.TryAddSingleton<IJsonSerializer, NewtonsoftJsonSerializer>();
            services.TryAddSingleton<ISettingsSender, HttpSettingSender>();
            services.AddMemoryCache();
            services.AddWebSocketServer(Configuration);
            //services.Configure<AirCCModel>(Configuration.GetSection("AirCC"));
            services.AddControllers();
            services.AddMapper(typeof(AutoMapperProfile));
            services.AddSqlServerDbContext<AirCCDbContext>(registerOption =>
            {
                registerOption.RegisterRepositories();
                registerOption.RegisterUnitOfWork();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
