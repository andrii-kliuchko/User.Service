using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using User.Service.Controllers;
using User.Service.Models;

namespace User.Service
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
            string storageModeVariable = Environment.GetEnvironmentVariable("STORAGE_MODE").ToUpper();
            if (storageModeVariable.Equals("LOCAL"))
            {
                services.AddScoped<IUserRepository, LocalUserRepository>();
            }
            else if (storageModeVariable.Equals("REMOTE"))
            {
                services.AddScoped<IUserRepository, RemoteUserRepository>();
                string connectionString = Configuration.GetConnectionString("RemoteDatabase");
                services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));
            }

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
