using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineStore.Legacy.Models;

namespace OnlineStore.Legacy.Services
{
    public interface IInventoryService
    {
        Task<List<Product>> GetAllProducts();
        Task AdjustInventory(string productId, int quantity);
    }
}