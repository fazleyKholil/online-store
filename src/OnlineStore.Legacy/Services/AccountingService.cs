using System.Threading.Tasks;
using App.Metrics;
using OnlineStore.Legacy.DataAccess;
using OnlineStore.Legacy.Models;

namespace OnlineStore.Legacy.Services
{
    public class AccountingService : IAccountingService
    {
        private readonly IMetrics _metrics;
        private readonly ILedgerDb _ledgerDb;

        public AccountingService(IMetrics metrics, ILedgerDb ledgerDb)
        {
            _metrics = metrics;
            _ledgerDb = ledgerDb;
        }

        public async Task<Ledger> Ledger()
        {
            return _ledgerDb.GetLedger();
        }

        public async Task<Ledger> PerformAccounting(Sale sale)
        {
            //calculate profit
            var totalCost = 0M;
            var totalSales = 0M;
            var totalProfit = 0M;

            foreach (var product in sale.Products)
            {
                totalCost += product.BuyingPrice;
                totalSales += product.SellingPrice;
                totalProfit += (product.SellingPrice - product.BuyingPrice);
            }

            //calculate tax amount varies by product
            var tax = totalCost * (15M / 100M);
            var netProfit = totalProfit - tax;

            //apply to ledger assume it takes several process/steps for demo purposed
            await _ledgerDb.UpdateLedger(totalSales, totalCost, totalProfit, netProfit);

            return _ledgerDb.GetLedger();
        }
    }
}