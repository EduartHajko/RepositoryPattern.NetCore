using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using CoreUnityOfWork.Hubs;
using CoreUnityOfWork.Jobs;
using CoreUnityOfWork.Midlewares;
using CoreUnityOfWork.Scheduler;
using CrystalQuartz.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using unityOfWork.BuissnesServices.IServices;
using unityOfWork.BuissnesServices.Services;
using unityOfWork.DTO;
using unityOfWork.Repository.CRUD;
using unityOfWork.Repository.ICRUD;

namespace CoreUnityOfWork
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
            services.AddDbContext<NetCoreDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddCors();
            services.AddScoped<DbContext, NetCoreDbContext>();
            services.AddScoped<ICrudRepository<int, Values>, ValuesRepsitory>();
            services.AddScoped<IValuesService, ValuesService>();
            services.AddSignalR();
            services.AddQuartz(typeof(ScheduledJob));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseWrapper();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseQuartz();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessageHub>("/SignalR");
            });
            
           

        }
       
        
    }
}
