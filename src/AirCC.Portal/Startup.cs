using AirCC.Portal.AppService;
using AirCC.Portal.AppService.Clients;
using AirCC.Portal.EntityFramework;
using AirCC.Portal.Infrastructure.WsServers;
using AirCC.Portal.WebServers;
using BCI.Extensions.AutoMapper;
using BCI.Extensions.Core.DI;
using BCI.Extensions.Core.ObjectMapping;
using BCI.Extensions.EFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

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
            services.AddLogging(configure => configure.AddConsole());
            services.AddHttpContextAccessor();
            services.AddTypes().AddOptions(Configuration, true);
            services.AddSqlServerDbContext<AirCCDbContext>(registerOption =>
            {
                registerOption.RegisterRepositories();
                registerOption.RegisterUnitOfWork();
            });
            services.TryAddSingleton<IEntityMappingManager, EntityMappingManager>();
            services.TryAddSingleton<ISettingsSender, WsSocketSettingsSender>();
            services.AddMemoryCache();
            services.AddWebSocketServer(Configuration);
            services.AddControllers().AddNewtonsoftJson();
            services.AddMapper(typeof(AutoMapperProfile));
            AddSwagger(services);
            //services.AddServerSideBlazor();
            //services.AddControllersWithViews();
            //services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            UseSwagger(app);

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                //endpoints.MapBlazorHub();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Air CC", Version = "V1" });
                options.CustomSchemaIds(x => x.FullName);
            });
        }

        private void UseSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Air CC API");
            });
        }
    }
}
