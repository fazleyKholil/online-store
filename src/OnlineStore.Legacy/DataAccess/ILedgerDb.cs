using System.Threading.Tasks;
using OnlineStore.Legacy.Models;

namespace OnlineStore.Legacy.DataAccess
{
    public interface ILedgerDb
    {
        Ledger GetLedger();

        Task<Ledger> UpdateLedger(decimal totalSales, decimal totalCost, decimal totalProfit, decimal netProfit);
    }
}