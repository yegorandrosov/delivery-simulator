using DeliverySimulator.Kitchen.Interfaces;
using DeliverySimulator.Kitchen.Models;
using System.Timers;

namespace DeliverySimulator.Kitchen.Shelves.ShelfTimers
{
    /// <summary>
    /// Simple timer factory that is based on order max age for specific shelf
    /// </summary>
    public class OrderDeterriorationTimerFactory : IOrderTimerFactory
    {
        /// <summary>
        /// Get timer based on order max age for specific shelf
        /// </summary>
        /// <param name="shelf">Shelf which contains the order</param>
        /// <param name="order">Order</param>
        /// <returns>Timer with calculated interval</returns>
        public Timer Create(KitchenShelf shelf, ShelfOrder order)
        {
            var timer = new Timer(order.GetMaxOrderAge(shelf.DecayModifier) * 1000);

            timer.AutoReset = false;

            return timer;
        }
    }
}
