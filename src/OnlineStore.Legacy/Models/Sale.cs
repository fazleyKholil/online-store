using System.Collections.Generic;

namespace OnlineStore.Legacy.Models
{
    public class Sale
    {
        public string SaleId { get; set; }

        public List<Product> Products { get; set; }

        public decimal ShippingCost { get; set; }

        public Sale()
        {
            Products = new List<Product>();
        }
    }
}