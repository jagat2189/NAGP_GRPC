


# Steps to run the solution:
1. Clone the project
2. Start RabbitMQ on Docker with the command
	docker run -d --hostname my-rabbit --name dev-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management
	This will start rabbitmq on localhost with the default username/password.
3. Open MicroservicesSolution.sln and run the solution in visual studio. This will start all 4 project i.e Product Service, Order Microservice, Notification1 serive, notification2 service
4. ProductService is configured as GRPC Client and it will present a swagger ui in the browser to call the api endpoints for viewing available products and creating/updating orders. The product service will invoke the PlaceOrder and UpdateOrder methods on the OrdersService via GRPC.



**Pre-Requisite**
- Docker should be installed and running on the system.
- .net 6 SDK and VS 2022 is required.
