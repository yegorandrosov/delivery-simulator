using DeliverySimulator.Kitchen.Models;
using DeliverySimulator.Shared;
using DeliverySimulator.Shared.Models;
using System;
using System.Timers;

namespace DeliverySimulator.Kitchen.Shelves.ShelfTimers
{
    /// <summary>
    /// Simple timer factory with random interval set in predefined boundaries
    /// </summary>
    public class CourierTimerFactory : IOrderTimerFactory
    {
        private Random random = new Random();

        /// <summary>
        /// Create timer with random interval set in predefined boundaries
        /// </summary>
        /// <param name="shelf">This parameter is ignored</param>
        /// <param name="order">This parameter is ignored</param>
        /// <returns></returns>
        public Timer Create(KitchenShelf shelf, ShelfOrder order)
        {
            var courierArrivalDelay = random.Next(
                AppSettings.Instance.AppConfig.Shelves.Timers.CourierTimerMinDelay,
                AppSettings.Instance.AppConfig.Shelves.Timers.CourierTimerMaxDelay);
            var timer = new Timer(TimeSpan.FromSeconds(courierArrivalDelay).TotalMilliseconds);

            timer.AutoReset = false;

            return timer;
        }
    }
}
