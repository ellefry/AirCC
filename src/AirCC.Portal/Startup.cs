#region usings

using System.Threading.Tasks;
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
using BCI.Extensions.Mvc;
using BCI.Extensions.Newtonsoft;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
#endregion

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
            services.TryAddSingleton<IJsonSerializer, NewtonsoftJsonSerializer>();
            services.AddMemoryCache();
            services.AddWebSocketServer(Configuration);
            services.AddControllers(options =>
            {
                options.Filters.Add<HttpExceptionFilter>();
            }).AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                opt.SerializerSettings.Converters.Add(new StringEnumConverter { AllowIntegerValues = false, NamingStrategy = null });
                var resover = opt.SerializerSettings.ContractResolver;
                if (resover != null)
                {
                    var res = resover as DefaultContractResolver;
                    res.NamingStrategy = null;
                }
            });
            services.AddMapper(typeof(AutoMapperProfile));
            AddSwagger(services);
            //services.AddServerSideBlazor();
            //services.AddControllersWithViews();
            //services.AddRazorPages();

            //security
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                   .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                   {
                       o.LoginPath = new PathString("/Login");
                       //o.AccessDeniedPath = new PathString("/Error/Forbidden");
                       o.Events.OnRedirectToLogin = context =>
                       {
                           if (context.Request.Path.Value.StartsWith("/api"))
                           {
                               context.Response.Clear();
                               context.Response.StatusCode = 401;
                               return Task.FromResult(0);
                           }

                           context.Response.Redirect(context.RedirectUri);
                           return Task.FromResult(0);
                       };
                   });
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

            app.UseAuthentication();
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
