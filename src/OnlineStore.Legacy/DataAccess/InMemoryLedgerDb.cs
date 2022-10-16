using System.Threading.Tasks;
using App.Metrics;
using OnlineStore.Legacy.Models;

namespace OnlineStore.Legacy.DataAccess
{
    public class InMemoryLedgerDb : ILedgerDb
    {
        private readonly IMetrics _metrics;
        private static readonly object LedgerLock = new object();
        private static Ledger _ledger;

        public InMemoryLedgerDb(IMetrics metrics)
        {
            _metrics = metrics;
            _ledger = new Ledger {NetProfit = 0, TotalCosts = 0, TotalProfit = 0, TotalSales = 0};
        }

        public Ledger GetLedger()
        {
            return _ledger;
        }

        public async Task<Ledger> UpdateLedger(decimal totalSales, decimal totalCost, decimal totalProfit, decimal netProfit)
        {
            lock (LedgerLock)
            {
                //simulate real life latency
                 Task.Delay(ExternalServicesConst.LedgerLatency).Wait();

                _ledger.TotalSales += totalSales;
                _ledger.TotalCosts += totalCost;
                _ledger.TotalProfit += totalProfit;
                _ledger.NetProfit += netProfit;

                return _ledger;
            }
        }
    }
}