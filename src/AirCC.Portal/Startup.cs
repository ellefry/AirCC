using AirCC.Portal.AppService;
using AirCC.Portal.AppService.Clients;
using AirCC.Portal.EntityFramework;
using AirCC.Portal.Infrastructure.WsServers;
using AirCC.Portal.WebServers;
using BCI.Extensions.AutoMapper;
using BCI.Extensions.Core.DI;
using BCI.Extensions.Core.Json;
using BCI.Extensions.Core.ObjectMapping;
using BCI.Extensions.EFCore;
using BCI.Extensions.Newtonsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

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
            services.AddSqlServerDbContext<AirCCDbContext>(registerOption =>
            {
                registerOption.RegisterRepositories();
                registerOption.RegisterUnitOfWork();
            });
            services.TryAddSingleton<IEntityMappingManager, EntityMappingManager>();
            services.TryAddSingleton<IJsonSerializer, NewtonsoftJsonSerializer>();
            services.TryAddSingleton<ISettingsSender, WsSocketSettingsSender>();
            services.AddMemoryCache();
            services.AddWebSocketServer(Configuration);
            services.AddControllers();
            services.AddMapper(typeof(AutoMapperProfile));
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
