namespace OnlineStore.Legacy.Models
{
    public class Ledger
    {
        public decimal TotalSales { get; set; } // 1000

        public decimal TotalCosts { get; set; } //600

        public decimal TotalProfit { get; set; } //400

        public decimal NetProfit { get; set; } //400-taxes
    }
}