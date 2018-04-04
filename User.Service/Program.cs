using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace User.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs\\user.service.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Log.Logger = logger;
            Log.Warning("Logger configured");
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
