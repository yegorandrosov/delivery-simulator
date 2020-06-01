using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.Shared
{
    public class AppConfiguration
    {
        public OrderEmitterConfiguration OrderEmitter { get; set; }

        public RabbitMQConfiguration RabbitMQ { get; set; }

        public ShelvesConfiguration Shelves { get; set; }
    }

    public class OrderEmitterConfiguration
    {
        public double NumberOfOrdersPerSecond;
    }

    public class RabbitMQConfiguration
    {
        public string Host;

        public string KitchenQueueName;
        public string EventQueueName;
        public string KitchenTerminationQueueName;
        public string EventLogDisplayTerminationQeueueName;
    }

    public class ShelvesConfiguration
    {
        public int RegularShelfDecayModifier;
        public int RegularShelfCapacity;

        public int OverflowShelfDecayModifier;
        public string OverflowShelfName;
        public int OveflowShelfCapacity;

        public Timers Timers { get; set; }

        public List<DefaultShelf> DefaultShelves { get; set; }

    }

    public class Timers
    {
        public int CourierTimerMinDelay;
        public int CourierTimerMaxDelay;
    }

    public class DefaultShelf
    {
        public string Name { get; set; }

        public int Capacity { get; set; }

        public int DecayModifier { get; set; }
    }
}
