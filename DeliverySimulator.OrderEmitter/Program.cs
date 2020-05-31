using DeliverySimulator.OrderEmitter.OrderProviders;
using DeliverySimulator.Shared;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.OrderEmitter
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonFileOrderProvider = new JsonFileOrderProvider("orders.json");

            using (var queuePublisher = new QueuePublisher(Configuration.RabbitMQ.KitchenQueueName))
            {
                queuePublisher.Published += (sender, ea) =>
                {
                    Console.WriteLine($" [x] Sent order {ea.Message}");
                };

                var emitOrdersService = new EmitOrdersService(queuePublisher, jsonFileOrderProvider);

                emitOrdersService.OnOutOfOrders += EmitOrdersService_OnOutOfOrders;
                emitOrdersService.StartEmittingOrders();
                Console.ReadKey();
            }

        }

        private static void EmitOrdersService_OnOutOfOrders(object sender, EventArgs args)
        {
            Environment.Exit(0);
        }
    }
}
