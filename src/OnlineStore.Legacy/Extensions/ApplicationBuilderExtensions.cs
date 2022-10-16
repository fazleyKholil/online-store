using Microsoft.AspNetCore.Builder;

namespace OnlineStore.Legacy.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMetrics(this IApplicationBuilder app)
        {
            return app.Map("/_system", systemBuilder =>
                systemBuilder
                    .UsePingEndpoint()
                    .UseHealthEndpoint()
                    .UseMetricsTextEndpoint()
            );
        }
    }
}