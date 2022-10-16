namespace OnlineStore.Legacy.Models
{
    public class Shipping
    {
        public string ShippingMethod { get; set; }

        public string CompanyName { get; set; }

        public decimal Cost { get; set; }

        public int EstimatedDays { get; set; }
    }
}