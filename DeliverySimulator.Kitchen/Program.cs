using DeliverySimulator.Kitchen.Interfaces;
using DeliverySimulator.Kitchen.Models;
using DeliverySimulator.Kitchen.NotificationServices;
using DeliverySimulator.Kitchen.Renderers;
using DeliverySimulator.Kitchen.Shelves;
using DeliverySimulator.Kitchen.Shelves.ShelfTimers;
using DeliverySimulator.Shared;
using DeliverySimulator.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.Kitchen
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;

            using (var eventPublisher = new QueuePublisher(Configuration.RabbitMQ.EventQueueName))
            {
                var kitchenShelvesManager = new SynchronizedKitchenShelvesManager(
                    new RegularKitchenShelfFactory(),
                    new CourierTimerFactory(),
                    new OrderDeterriorationTimerFactory(),
                    new QueueNotificationService(eventPublisher));

                IRenderer renderer = new KitchenConsoleRenderer(kitchenShelvesManager);
                
                using (var renderEngine = new SimpleTimerRenderEngine(renderer, fps: 1))
                {
                    renderEngine.Start();

                    using (var kitchenOrdersConsumer = new KitchenQueueConsumer(new QueueConsumer(Configuration.RabbitMQ.KitchenQueueName), kitchenShelvesManager))
                    {
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
