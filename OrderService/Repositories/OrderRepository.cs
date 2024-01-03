using OrderService.Protos;

namespace OrderService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public string ProcessOrder(OrderRequest order)
        {
            return $"Order with order id {order.OrderId} has been placed successfully";
        }

        public string UpdateOrder(OrderRequest order)
        {
            return $"Order with order id {order.OrderId} has been updated successfully";
        }
    }
}
