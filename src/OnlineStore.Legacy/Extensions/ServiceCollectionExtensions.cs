using App.Metrics;
using App.Metrics.Formatters.Prometheus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineStore.Legacy.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMetrics(this IServiceCollection services, IConfiguration configuration, string applicationVersion = null)
        {
            var metricsConfigSection = configuration.GetSection(nameof(MetricsOptions));

            var metricsBuilder = AppMetrics.CreateDefaultBuilder()
                .Configuration.Configure(metricsConfigSection.AsEnumerable())
                .OutputMetrics.AsPrometheusPlainText();

            metricsBuilder.Configuration.Configure(options =>
            {
                if (!string.IsNullOrWhiteSpace(applicationVersion))
                {
                    options.GlobalTags.Add("version", applicationVersion);
                }
            });

            var metrics = metricsBuilder.Build();

            services.AddMetrics(metrics);

            services.AddMetricsEndpoints(o =>
            {
                o.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter(new MetricsPrometheusOptions
                {
                    NewLineFormat = NewLineFormat.Unix
                });
            });

            return services;
        }
    }
}