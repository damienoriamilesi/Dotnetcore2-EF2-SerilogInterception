using System.Diagnostics;
using BethaniePieShop.Controllers;
using Castle.DynamicProxy;
using Microsoft.IdentityModel.Protocols;
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

            // Log Method, Arguments, execution time...
            if (sw.Elapsed.TotalSeconds > 5)
            {
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