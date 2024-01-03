using JsonDb;

using Microsoft.AspNetCore.Mvc;

using ProductService.Protos;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly OrderHandlerService.OrderHandlerServiceClient orderHandlerClient;
        private readonly IJsonDb db;

        public ProductController(
            OrderHandlerService.OrderHandlerServiceClient orderHandlerClient,
            IJsonDbFactory jsonDbFactory)
        {
            this.orderHandlerClient = orderHandlerClient;
            db = jsonDbFactory.GetJsonDb();
        }

        [HttpPost("PlaceOrder")]
        public IActionResult PlaceOrder(int productId, string address, int quantity)
        {
            var product = db.GetCollectionAsync<Product>("products").Result.SingleOrDefault(x => x.ProductId == productId);
            if (product != null)
            {
                var orderRequest = new OrderRequest
                {
                    OrderId = new Random().Next(),
                    Product = new Product { ProductId = productId, Color = product.Color, Description = product.Description, Price = product.Price },
                    Address = address,
                    Quantity = quantity
                };

                var response = orderHandlerClient.PlaceOrder(orderRequest);

                return Ok(new { orderId = orderRequest.OrderId, status = response.Status });
            }
            else
            {
                return BadRequest($"The Prodcut with ID:{productId} does not exist.");
            }
        }

        [HttpPost("UpdateOrder")]
        public IActionResult UpdateOrder(int orderId, int productId, string shippingAddress, int quantity)
        {
            var product = db.GetCollectionAsync<Product>("products").Result.SingleOrDefault(x => x.ProductId == productId);
            if (product != null)
            {
                var orderRequest = new OrderRequest
                {
                    OrderId = orderId,
                    Product = new Product { ProductId = productId, Color = product.Color, Description = product.Description, Price = product.Price },
                    Address = shippingAddress,
                    Quantity = quantity
                };

                var response = orderHandlerClient.UpdateOrder(orderRequest);

                return Ok(new { status = response.Status });
            }
            else
            {
                return BadRequest($"The Prodcut with ID:{productId} does not exist.");
            }
        }

        [HttpGet("FetchAllProducts")]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await db.GetCollectionAsync<Product>("products");
            return products;
        }
    }
}