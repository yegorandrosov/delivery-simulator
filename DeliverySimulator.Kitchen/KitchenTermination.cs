using DeliverySimulator.Kitchen.Models;
using DeliverySimulator.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DeliverySimulator.Kitchen
{
    public class KitchenTerminationProcess
    {
        public KitchenTerminationProcess(KitchenShelvesManager kitchenShelves)
        {
            var timer = new Timer();

            timer.Interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
            timer.AutoReset = true;

            timer.Elapsed += (sender, args) =>
            {
                // wait for kitchen to stop working
                if (kitchenShelves.All(x => x.Orders.Count == 0))
                {
                    using (var eventTerminationQueuePublisher = new QueuePublisher(AppSettings.Instance.AppConfig.RabbitMQ.EventLogDisplayTerminationQeueueName))
                    {
                        eventTerminationQueuePublisher.Publish(new object());
                    }

                    Environment.Exit(0);
                }
            };

            timer.Start();
        }
    }
}
