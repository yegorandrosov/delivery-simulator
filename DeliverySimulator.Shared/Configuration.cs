using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.Shared
{
    public static class Configuration
    {
        public static class OrderEmitter
        {
            public const double NumberOfOrdersPerSecond = 2;
        }

        public static class RabbitMQ
        {
            public const string Host = "localhost";

            public const string KitchenQueueName = "kitchen";
            public const string EventQueueName = "events";
        }

        public static class Shelves
        {
            public const int RegularShelfDecayModifier = 1;
            public const int RegularShelfCapacity = 10;

            public const int OverflowShelfDecayModifier = 2;
            public const string OverflowShelfName = "overflow";
            public const int OveflowShelfCapacity = 15;

            public static class Timers
            {
                public const int CourierTimerMinDelay = 6;
                public const int CourierTimerMaxDelay = 10;
            }
        }
    }
}
