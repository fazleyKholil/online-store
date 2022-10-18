using System;
using App.Metrics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using OnlineStore.Legacy.DataAccess;
using OnlineStore.Legacy.Extensions;
using OnlineStore.Legacy.Services;

namespace OnlineStore.Legacy
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
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "OnlineStore.Legacy", Version = "v1"}); });
            services.AddMetrics(Configuration, ReflectionUtils.GetAssemblyVersion<Program>());
            services.AddHealth();
            services.AddHealthChecks();
            services.AddAppMetricsHealthPublishing();

            services.AddSingleton<IInventoryDb, InventoryDb>();
            services.AddSingleton<ILedgerDb, InMemoryLedgerDb>();

            services.AddTransient<IShippingService, ShippingService>();
            services.AddTransient<IAccountingService, AccountingService>();
            services.AddTransient<IInventoryService, InventoryService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMetrics metrics)
        {
            metrics.IncrementOperation("legacy_app_count");

            var serverId = Guid.NewGuid().ToString();
            var pathBase = Configuration.GetValue<string>("ASPNETCORE_PATHBASE");
            if (!string.IsNullOrWhiteSpace(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineStore.Legacy v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/_system/health", new HealthCheckOptions
                {
                    ResponseWriter = HealthReports.FormatAsync
                });

                endpoints.MapGet("/_system/id", async context => { await context.Response.WriteAsync($"ServerId : {serverId}"); });
            });

            app.UseMetrics();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}