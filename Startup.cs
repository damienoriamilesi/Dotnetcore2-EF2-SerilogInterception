using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using BethaniePieShop.Controllers;
using BethaniePieShop.Models;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BethaniePieShop
{
    public class Startup
    {
        public readonly IConfiguration Configuration;
        
        public Startup(IConfiguration config)
        {
            Configuration = config;
            //ContainerBuilder = containerBuilder;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {     
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Configuration["ConnectionString:DefaultConnection"]));

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            #region Dependency Injection => Scan Assembly

            var assemblies = new [] { Assembly.GetAssembly(typeof(PieRepository)) };
            var types = assemblies.SelectMany(assembly => assembly.GetTypes().Where(type => !type.IsInterface && type.GetInterfaces().Any(x => x.FullName.Contains("IRepository"))));
            
            foreach (Type implementationType in types)
            {
                foreach(Type interfaceType in implementationType.GetInterfaces())
                {
                    services.AddScoped(interfaceType, implementationType);   
                }
            }

            // Could be scanned too
            services.AddTransient<DbSeeder>();
            services.AddTransient<LoggerInterceptor>();

            #endregion            
        
            services.AddLogging(logCfg => 
                                {
                                    logCfg.AddSeq(Configuration.GetSection("Seq"));
                                });

            services.AddAutofac();

            services.AddMvc().AddControllersAsServices();
            
            var b = new Autofac.ContainerBuilder();
            b.Populate(services); 

            var controllers = Assembly.GetExecutingAssembly().GetTypes()
                                    .Where(type => type.BaseType ==  typeof(BaseController)).ToArray();
            b.RegisterTypes(controllers).AsSelf().EnableClassInterceptors().InterceptedBy(typeof(LoggerInterceptor));
            
            return new AutofacServiceProvider(b.Build());        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
        {
            app.UseStaticFiles(); //==> www files
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }

            app.UseAuthentication();

            app.UseMvc(cfg =>
            {
                cfg.MapRoute("pie", "{controller=pie}/{action=index}/{id?}");
            });            
        }
    }
}
