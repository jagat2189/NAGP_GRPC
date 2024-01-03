
## Overview

This microservices solution comprises four projects:

1. **Product Service**: Acts as a gRPC client with a Swagger UI for placing and updating orders. Communicates with the Order Service via gRPC and publishes events using RabbitMQ.

2. **Order Service**: A gRPC server that handles order placement and updates. Publishes events using RabbitMQ.

3. **Notification Service 1**: Listens for order creation events on the "order_creation_exchange" (Fanout Exchange) and displays notifications.

4. **Notification Service 2**: Listens for both order creation and update events. Binds to "order_creation_exchange" (Fanout Exchange) and "order_update_exchange" (Topic Exchange) and displays notifications.


## Running the Solution

### Prerequisites

- .NET SDK
- RabbitMQ server
- Docker

### Steps
1. **Clone the Repository:**
	 ```bash
   git clone https://github.com/jagat2189/NAGP_GRPC.git
   cd your-repo
2. **Run RabbitMQ:** 
	Start RabbitMQ on Docker with the command
	docker run -d --hostname rmq --name rebbit-server -p 15672:15672 -p 5672:5672 rabbitmq:3-management
	This will start rabbitmq on localhost with the default username/password.
3. **Build and Run Projects:** 
Open MicroservicesSolution.sln and run the solution in visual studio. This will start all four project i.e Product Service, Order Microservice, Notification1 serive, notification2 service
4. **Access Swagger UI:**
Open your browser and navigate to the Swagger UI for the Product Service: https://localhost:7196/swagger/index.html
Use the Swagger UI to place and update orders. This will trigger events and notify the corresponding services.
5. **View Notifications**
Check the console outputs of Notification Service 1 and Notification Service 2 to see notifications for order creation and updates.

