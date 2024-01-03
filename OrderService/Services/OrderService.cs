using Grpc.Core;

using OrderService.Protos;
using OrderService.RabbitMQ;
using OrderService.Repositories;

using RabbitMQ.Client;

namespace OrderService.Services
{
    public class OrderService : OrderHandlerService.OrderHandlerServiceBase
    {
        private readonly ILogger<OrderService> logger;

        private readonly IRabbitMQService rabbitMQ;

        private readonly IOrderRepository orderRepository;

        public OrderService(ILogger<OrderService> logger, IRabbitMQService rabbitMQ, IOrderRepository orderRepository)
        {
            this.logger = logger;
            this.rabbitMQ = rabbitMQ;
            this.orderRepository = orderRepository;
        }

        public override async Task<OrderResponse> PlaceOrder(OrderRequest request, ServerCallContext context)
        {
            try
            {
                logger.LogInformation($"Received PlaceOrder request for OrderId: {request.OrderId}");

                var orderStatus = orderRepository.ProcessOrder(request);

                await rabbitMQ.PublishEvent("order_creation_exchange", ExchangeType.Fanout, string.Empty, request);

                logger.LogInformation($"Order processed successfully. OrderId: {request.OrderId}, Status: {orderStatus}");

                return await Task.FromResult(new OrderResponse
                {
                    Status = orderStatus
                });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error processing PlaceOrder request for OrderId: {request.OrderId}. Error: {ex.Message}");

                return new OrderResponse
                {
                    Status = "Error occurred while pocessing order"
                };
            }
        }

        public override async Task<OrderResponse> UpdateOrder(OrderRequest request, ServerCallContext context)
        {
            try
            {
                logger.LogInformation($"Received UpdateOrder request for OrderId: {request.OrderId}");

                var orderStatus = orderRepository.UpdateOrder(request);

                await rabbitMQ.PublishEvent("order_update_exchange", ExchangeType.Topic, "order", request);

                logger.LogInformation($"Order updated successfully. OrderId: {request.OrderId}, Status: {orderStatus}");

                return await Task.FromResult(new OrderResponse
                {
                    Status = orderStatus
                });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error processing UpdateOrder request for OrderId: {request.OrderId}. Error: {ex.Message}");

                return new OrderResponse
                {
                    Status = "Error occured while updating order"
                };
            }
        }
    }
}
