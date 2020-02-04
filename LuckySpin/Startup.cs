using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuckySpin.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;


namespace LuckySpin
{
    public class Startup
    {
     // Injects the Configuration used to read the database connection string from appsetting.json
         public IConfiguration Configuration { get; }
         public Startup(IConfiguration configuration) //Startup Constructor
         {
            Configuration = configuration;
         }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LuckySpinContext>(options => options.UseSqlServer(Configuration.GetConnectionString("LuckySpinDb")));
            services.AddMvc();
            services.AddTransient<Models.TextTransformService>();
            //TODO: Remove the Singleton Repository
            //services.AddSingleton<Models.Repository>();
            //TODO: Register the DataBase Context
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseDeveloperExceptionPage();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller}/{action}/{playerid:int?}",
                     defaults: new { controller = "Spinner", action = "Index" }
                );
            });
            app.UseStaticFiles();
        }
    }
}
