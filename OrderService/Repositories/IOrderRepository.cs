using OrderService.Protos;

namespace OrderService.Repositories
{
    public interface IOrderRepository
    {
        string ProcessOrder(OrderRequest order);

        string UpdateOrder(OrderRequest order);
    }
}
