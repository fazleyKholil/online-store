using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using OnlineStore.Legacy.Extensions;
using OnlineStore.Legacy.ExternalServices;
using OnlineStore.Legacy.Models;

namespace OnlineStore.Legacy.Services
{
    public class ShippingService :IShippingService
    {
        private readonly IMetrics _metrics;

        public ShippingService(IMetrics metrics)
        {
            _metrics = metrics;
        }

        public async Task<Shipping> GetShippingInformation(int distance, bool isExpress)
        {
            var shippingOptions = new List<Shipping>();

            using (_metrics.TimeOperation("request_external_shipping_service", "shipping_service"))
            {
                var dhlShipping = await RequestDhlShippingDetail(distance);
                shippingOptions.Add(dhlShipping);

                var fedexShipping = await RequestFedexShippingDetail(distance);
                shippingOptions.Add(fedexShipping);

                var aramexShipping = await RequestAramexShippingDetail(distance);
                shippingOptions.Add(aramexShipping);
            }

            var bestCost = shippingOptions.Min(s => s.Cost);
            var bestTimeArrival = shippingOptions.Min(s => s.EstimatedDays);


            return isExpress
                ? shippingOptions.FirstOrDefault(s => s.EstimatedDays == bestTimeArrival)
                : shippingOptions.FirstOrDefault(s => s.Cost == bestCost);
        }

        public async Task<Shipping> RequestDhlShippingDetail(int distance)
        {
            using (_metrics.TimeOperation("request_external_shipping_service_dhl", "shipping_service"))
            {
                //simulating DHL external service
                return await MockServer.SendShippingRequest("Dhl", distance);
            }
        }

        public async Task<Shipping> RequestFedexShippingDetail(int distance)
        {
            using (_metrics.TimeOperation("request_external_shipping_service_fedex", "shipping_service"))
            {
                //simulating Fedex external service
                return await MockServer.SendShippingRequest("Fedex", distance);
            }
        }

        public async Task<Shipping> RequestAramexShippingDetail(int distance)
        {
            using (_metrics.TimeOperation("request_external_shipping_service_aramex", "shipping_service"))
            {
                //simulating Aramex external service
                return await MockServer.SendShippingRequest("Aramex", distance);
            }
        }
    }
}