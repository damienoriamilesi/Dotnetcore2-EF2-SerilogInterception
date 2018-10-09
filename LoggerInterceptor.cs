using System.Collections.Generic;
using System.Diagnostics;
using BethaniePieShop.Controllers;
using Castle.DynamicProxy;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using Serilog;

namespace BethaniePieShop
{
    public class LoggerInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            invocation.Proceed();

            sw.Stop();

            // var s = JsonConvert.SerializeObject(invocation.Arguments, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });



            // Log Method, Arguments, execution time...
            if (sw.Elapsed.TotalSeconds > 5)
            {
                // TelemetryClient logClient = new TelemetryClient(new Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration("7b97ded0-325d-4c85-89f7-e88a23e51d39"));
                // logClient.TrackEvent("CustomEvent", 
                //                         new Dictionary<string, string>() 
                //                         { 
                //                             { "MethodName",  $"{invocation.Method.DeclaringType?.Name}/{invocation.Method.Name}"}, 
                //                             { "Arguments",  s},
                //                             { "ExecutionTime",  $"{sw.Elapsed.TotalSeconds} sec."}
                //                         });                
                                        
                if (invocation.Method.DeclaringType?.BaseType?.IsAssignableFrom(typeof(BaseController)) ?? false)
                {
                    LogHelper.Instance.Warning(
                        "LogError : {MethodName} {@Arguments} {ExecutionTime}",
                        $"{invocation.Method.DeclaringType?.Name}/{invocation.Method.Name}", invocation.Arguments, sw.Elapsed.TotalSeconds);
                }
                // else
                // {
                //     LogHelper.Instance.Warning(
                //                         "LogError : {MethodName} {ExecutionTime}",
                //                         $"{invocation.Method.DeclaringType?.Name}/{invocation.Method.Name}", sw.Elapsed.TotalSeconds);
                // }

                Log.CloseAndFlush();
            }


            // var logger = new LoggerConfiguration().WriteTo.Seq("http://localhost:5341").CreateLogger();

            // logger.Warning(
            //             "LogError : {MethodName} {@Arguments} {ExecutionTime}",
            //             $"{invocation.Method.DeclaringType?.Name}/{invocation.Method.Name}", invocation.Arguments, sw.Elapsed.TotalSeconds);

            // Log.CloseAndFlush();
        }
    }
}