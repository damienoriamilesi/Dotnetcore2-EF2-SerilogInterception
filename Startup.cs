using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BethaniePieShop.Models;
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
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {     
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Configuration["ConnectionString:DefaultConnection"]));

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            #region Dependency Injection => Scan Assembly

            var assemblies = new [] { Assembly.GetAssembly(typeof(PieRepository)) };
                    var types = assemblies.SelectMany(assembly => assembly.GetTypes().Where(type => !type.IsInterface && type.GetInterfaces().Any(x=>x.FullName.Contains("IRepository"))));
                    foreach (var implementationType in types)
                    {
                        foreach(var interfaceType in implementationType.GetInterfaces())
                        {
                            services.AddScoped(interfaceType, implementationType);
                        }
                    }

            // Could be scanned too
            services.AddTransient<DbSeeder>();

            // services.AddTransient<IPieRepository, PieRepository>(); // Each call get a new instance
            // services.AddTransient<IFeedbackRepository, FeedbackRepository>(); // Each call get a new instance
            // //services.AddScoped<IPieRepository, PieRepository>(); // Each call get a the existing instance in the HttpRequest Scope
            #endregion            
        
            services.AddLogging(logCfg=>{
                logCfg.AddSeq(Configuration.GetSection("Seq"));
            });

            services.AddMvc();
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

            //  app.Use((context, next) =>
            //                         Task.Run(()=>{
            //                             var sw = new Stopwatch();
            //                             sw.Start();
            //                             next.Invoke();
            //                             sw.Stop();
            //                             LogHelper.Instance.Information(
            //                                 "LogError : {MethodName} {ExecutionTime}",
            //                             $"{next.Method.Name}/{context.Request.Path}", sw.Elapsed.TotalSeconds );
            //                             // $"{next.Method.DeclaringType?.Name}/{next.Method.Name} {next.GetMethodInfo().Name}", sw.Elapsed.TotalSeconds );
            //                             //await context.Response.WriteAsync(String.Format("<!-- {0} ms -->", sw.ElapsedMilliseconds));
            //                         }));


            app.UseAuthentication();

            app.UseMvc(cfg =>
            {
                cfg.MapRoute("pie", "{controller=pie}/{action=index}/{id?}");
            });            
        }
    }
}
