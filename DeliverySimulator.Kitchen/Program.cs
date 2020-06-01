using DeliverySimulator.Kitchen.Interfaces;
using DeliverySimulator.Kitchen.Models;
using DeliverySimulator.Kitchen.NotificationServices;
using DeliverySimulator.Kitchen.Renderers;
using DeliverySimulator.Kitchen.Shelves;
using DeliverySimulator.Kitchen.Shelves.DataInitialization;
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
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;

            using (var terminationQueueConsumer = new QueueConsumer(AppSettings.Instance.AppConfig.RabbitMQ.KitchenTerminationQueueName))
            using (var eventPublisher = new QueuePublisher(AppSettings.Instance.AppConfig.RabbitMQ.EventQueueName))
            {
                var kitchenShelvesManager = new KitchenShelvesManager(
                    new CourierTimerFactory(),
                    new OrderDeterriorationTimerFactory(),
                    new QueueNotificationService(eventPublisher),
                    new ShelvesInitializationFromConfigFile());

                terminationQueueConsumer.Received += (sender, ea) =>
                {
                    var terminationProcess = new KitchenTerminationProcess(kitchenShelvesManager);
                };

                IRenderer renderer = new KitchenConsoleRenderer(kitchenShelvesManager);

                using (var renderEngine = new SimpleTimerRenderEngine(renderer, fps: 1))
                {
                    renderEngine.Start();

                    using (var kitchenOrdersConsumer = new KitchenQueueConsumer(
                        new QueueConsumer(AppSettings.Instance.AppConfig.RabbitMQ.KitchenQueueName),
                        kitchenShelvesManager))
                    {
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
