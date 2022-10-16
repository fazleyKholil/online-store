using System.Threading.Tasks;
using OnlineStore.Legacy.Models;

namespace OnlineStore.Legacy.Services
{
    public interface IShippingService
    {
        Task<Shipping> GetShippingInformation(int distance, bool isExpress);
    }
}