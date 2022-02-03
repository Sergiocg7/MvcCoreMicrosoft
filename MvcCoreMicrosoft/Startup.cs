using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcCoreMicrosoft.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreMicrosoft
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
            string cadena = this.Configuration.GetConnectionString("cadenaAspNetDb");
            string appid = this.Configuration.GetValue<string>("Authentication:Microsoft:ApplicationId");
            string secretkey = this.Configuration.GetValue<string>("Authentication:Microsoft:SecretKey");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(cadena));
            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddAuthentication().AddMicrosoftAccount(options =>
            {
                options.ClientId = appid;
                options.ClientSecret = secretkey;
            });
            services.AddControllersWithViews(options => options.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    );
            });

            //app.UseEndpoints(endpoints =>
            //{
                //endpoints.MapControllerRoute(
                    //name: "default",
                    //pattern: "{controller=Home}/{action=Index}/{id?}");
            //});
        }
    }
}
