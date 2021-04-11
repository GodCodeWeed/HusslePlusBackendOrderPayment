using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Data.SqlClient;
using HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Repository;
using HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Services;
using HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Data;
using Microsoft.EntityFrameworkCore;
using HUSTLEPLUS.BUYER.ORDER.MICROSERVICE.Utilities;
using System.IO;
using Hangfire;

namespace HUSTLEPLUS.SELLER.ORDER.MICROSERVICE
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
         //var connection =   services.Configure<ConnectionStrings>();
            services.AddOptions();

            services.AddDbContext<OrderDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("OrderConnectionString")));
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("OrderConnectionString")));
            services.AddHangfireServer();
         
            services.AddControllers();
            services.AddSingleton<IOrders, Order>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HUSTLEPLUS.SELLER.ORDER.MICROSERVICE", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseHangfireDashboard("/mydashboard");

                //    RecurringJob.AddOrUpdate<IOrders>(x =>
                //  x.FufillmentCronJOb(), Cron.Minutely);
                RecurringJob.AddOrUpdate<IOrders>(x =>
                    x.ReconciliationAsync(), "*/2 * * * *");


                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HUSTLEPLUS.SELLER.ORDER.MICROSERVICE v1"));
            }

          //  app.UseHttpsRedirection();
           // app.UseSql//();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
