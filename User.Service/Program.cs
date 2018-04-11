using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using User.Service.Data;

namespace User.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            // Initializing logger
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs\\user.service.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Log.Logger = logger;
            Log.Warning("Logger configured");

            if (Environment.GetEnvironmentVariable("STORAGE_MODE").ToUpper() == "REMOTE")
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<UserContext>();
                        DbInitializer.Initialize(context);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "An error occured while seeding the database");
                    }
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
