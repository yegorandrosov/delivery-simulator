using DeliverySimulator.Kitchen.Models;
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
    public class KitchenQueueConsumer : IDisposable
    {
        private readonly QueueConsumer queueConsumer;
        private readonly KitchenShelvesManager shelvesManager;

        public KitchenQueueConsumer(QueueConsumer queueConsumer, KitchenShelvesManager shelvesManager)
        {
            this.queueConsumer = queueConsumer ?? throw new ArgumentNullException(nameof(queueConsumer));
            this.shelvesManager = shelvesManager ?? throw new ArgumentNullException(nameof(shelvesManager));

            queueConsumer.Received += (sender, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                var order = JsonConvert.DeserializeObject<Order>(message);

                shelvesManager.AddOrder(order);
            };
        }

        public void Dispose()
        {
            queueConsumer?.Dispose();
        }
    }
}
