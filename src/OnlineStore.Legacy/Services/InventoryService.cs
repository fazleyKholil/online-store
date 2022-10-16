using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics;
using OnlineStore.Legacy.DataAccess;
using OnlineStore.Legacy.Extensions;
using OnlineStore.Legacy.Models;

namespace OnlineStore.Legacy.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IMetrics _metrics;
        private readonly IInventoryDb _inventoryDb;

        public InventoryService(IMetrics metrics, IInventoryDb inventoryDb)
        {
            _metrics = metrics;
            _inventoryDb = inventoryDb;
        }

        public Task<List<Product>> GetAllProducts()
        {
            using (_metrics.TimeOperation("get_all_products", "inventory_service"))
            {
                return _inventoryDb.GetAll();
            }
        }

        public async Task AdjustInventory(string productId, int quantity)
        {
            using (_metrics.TimeOperation("update_stock", "inventory_service"))
            {
                await _inventoryDb.UpdateStock(productId, quantity);
                
                //other inventory related process like packaging box, goodies, etc tha will be bundled in the order but is optional
                await Task.Delay(ExternalServicesConst.InventoryOtherProcessLatency);
            }
        }
    }
}