using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NET_CORE.Models;
using OnionCore.Core.IApplicationService;
using OnionCore.Core.Interfaces;
using OnionCore.Repo.DataRepo;
using OnionCore.Repo.Repo;
namespace NET_CORE
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
            //DepencyInjection

            //services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<IDataRepo, DataRepo>();
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IDatabaseFactory, DatabaseFactory>();

            services.AddOptions();
            //Scan an assembly for components
            //register assembly
            services.Scan(scan => scan
                .FromAssemblies(typeof(ITestService).GetTypeInfo().Assembly)
                .AddClasses()
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            //ConnectionStrings connectionStrings = new ConnectionStrings();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //PWA .NET Core -- https://medium.com/beginners-guide-to-mobile-web-development/introduction-to-pwa-in-asp-net-core-application-da96c7cc4918
            services.AddProgressiveWebApp();
            services.AddServiceWorker();
            //services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            //connectionStrings.conn = Configuration.GetValue<string>("connectionString");
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
