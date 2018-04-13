using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using slogram.Models;

namespace slogram
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
            services.AddMvc();

            var dbHost = Configuration["vcap:services:mysql:0:credentials:host"];
            var dbUsername = Configuration["vcap:services:mysql:0:credentials:username"];
            var dbPassword = Configuration["vcap:services:mysql:0:credentials:password"];
            var dbDatabase = Configuration["vcap:services:mysql:0:credentials:name"];

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddDbContext<MvcPhotoContext>(options => 
            options.UseMySql("Server=" + dbHost + ";User Id=" + dbUsername + ";Password=" + dbPassword + ";Database=" + dbDatabase));
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
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Photos}/{action=Index}/{id?}");
            });

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<MvcPhotoContext>();
                context.Database.Migrate();
            }
        }
    }
}
