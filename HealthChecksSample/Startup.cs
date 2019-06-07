using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HealthChecksSample.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace HealthChecksSample
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddEntityFrameworkNpgsql()
               .AddDbContext<EliteAttachmentContext>(opt =>
               {
                   opt.UseNpgsql(Configuration.GetSection("ConnectionConfiguration:ConnectionString").Value, npgsqlOptionsAction: sqlOption =>
                   {
                       sqlOption.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                       sqlOption.EnableRetryOnFailure(maxRetryCount: 3,
                           maxRetryDelay: TimeSpan.FromSeconds(10), errorCodesToAdd: null);
                   });
               },
               ServiceLifetime.Scoped);

            services.AddHealthChecks()
                .AddCheck("MyDB", new HealthCheckBuilderPostgreSql(Configuration.GetSection("ConnectionConfiguration:ConnectionString").Value));

            services.AddHealthChecksUI();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHealthChecks("/healthcheck");
            app.UseHealthChecksUI(config => config.UIPath = "/hc-ui");
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
