using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineStore.Legacy.Extensions;
using OnlineStore.Legacy.Models;
using OnlineStore.Legacy.Services;

namespace OnlineStore.Legacy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LegacyController : ControllerBase
    {
        private readonly IMetrics _metrics;
        private readonly ILogger<LegacyController> _logger;
        private readonly IShippingService _shippingService;
        private readonly IAccountingService _accountingService;
        private readonly IInventoryService _inventoryService;

        public LegacyController(ILogger<LegacyController> logger
            , IMetrics metrics
            , IShippingService shippingService
            , IAccountingService accountingService
            , IInventoryService inventoryService)
        {
            _logger = logger;
            _metrics = metrics;
            _shippingService = shippingService;
            _accountingService = accountingService;
            _inventoryService = inventoryService;
        }

        [HttpGet]
        [Route("products")]
        public async Task<List<Product>> GetProducts()
        {
            return await _inventoryService.GetAllProducts();
        }

        [HttpGet]
        [Route("ledger")]
        public async Task<Ledger> GetLedger()
        {
            return await _accountingService.Ledger();
        }

        [HttpPost]
        [Route("order")]
        public async Task<IActionResult> Order(OrderRequest request)
        {
            _metrics.IncrementOperation("order_count", "legacy_controller");

            try
            {
                using (_metrics.TimeOperation("process_order", "legacy_controller"))
                {
                    //process order
                    var sale = new Sale();

                    //get products
                    var products = await _inventoryService.GetAllProducts();

                    //set related product in sale 
                    var total = 0M;
                    foreach (var product in request.Products)
                    {
                        var relatedProduct = products.First(p => p.ProductId == product.ProductId);
                        relatedProduct.Quantity = product.Quantity;
                        sale.Products.Add(relatedProduct);
                        total += relatedProduct.SellingPrice * product.Quantity;
                    }

                    //get shipping details
                    var shipping = await _shippingService.GetShippingInformation(request.ShippingDistance, request.IsShippingExpress);

                    sale.ShippingCost = shipping.Cost;

                    //update inventory
                    using (_metrics.TimeOperation("update_inventory", "legacy_controller_order"))
                    {
                        foreach (var product in sale.Products)
                        {
                            await _inventoryService.AdjustInventory(product.ProductId, product.Quantity);
                        }
                    }

                    //perform accounting 
                    using (_metrics.TimeOperation("perform_accounting", "legacy_controller_order"))
                    {
                        await _accountingService.PerformAccounting(sale);
                    }

                    //build response
                    var response = new SaleResponse
                    {
                        Total = total + sale.ShippingCost,
                        ShippingInformation = shipping,
                        Products = new List<object> {sale.Products.Select(p => new {p.Description, p.Quantity})}
                    };

                    _metrics.IncrementOperation("order_success_count", "legacy_controller");

                    return Ok(response);
                }
            }
            catch (Exception e)
            {
                _metrics.IncrementOperation("order_error_count", "legacy_controller");
                return BadRequest(e);
            }
        }
    }
}