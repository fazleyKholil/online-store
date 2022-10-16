using System.Threading.Tasks;
using OnlineStore.Legacy.Models;

namespace OnlineStore.Legacy.Services
{
    public interface IAccountingService
    {
        Task<Ledger> Ledger();
        Task<Ledger> PerformAccounting(Sale sale);
    }
}