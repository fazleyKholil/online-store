using System.Collections.Generic;

namespace OnlineStore.Legacy.Models
{
    public class SaleResponse
    {
        public decimal Total { get; set; }

        public List<object> Products { get; set; }

        public Shipping ShippingInformation { get; set; }
    }
}