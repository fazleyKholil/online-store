using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineStore.Legacy.Models;

namespace OnlineStore.Legacy.DataAccess
{
    public interface IInventoryDb
    {
        Task<List<Product>> GetAll();

        Task UpdateStock(string productId, int quantity);
    }
}