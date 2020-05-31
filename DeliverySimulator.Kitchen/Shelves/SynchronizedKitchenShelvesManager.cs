using DeliverySimulator.Kitchen.Models;
using DeliverySimulator.Kitchen.Shelves.Notifications;
using DeliverySimulator.Kitchen.Shelves.ShelfTimers;
using DeliverySimulator.Shared.Models;
using System.Timers;

namespace DeliverySimulator.Kitchen.Shelves
{
    /// <summary>
    /// Synchronized version of <see cref="KitchenShelvesManager"/>
    /// </summary>
    public class SynchronizedKitchenShelvesManager : KitchenShelvesManager
    {
        private readonly object _syncLock = new object();

        public SynchronizedKitchenShelvesManager(IKitchenShelfFactory kitchenShelfFactory, IOrderTimerFactory courierTimerFactory, IOrderTimerFactory deterriorationTimerFactory, IShelfNotificationService shelfNotificationService) 
            : base(kitchenShelfFactory, courierTimerFactory, deterriorationTimerFactory, shelfNotificationService)
        {
        }

        /// <summary>
        /// Synchronized version of <see cref="KitchenShelvesManager.AddOrder(Order)"/>. Adds order to specific or "overflow" shelf
        /// </summary>
        /// <param name="order">Order to be added</param>
        public override void AddOrder(Order order)
        {
            lock (_syncLock)
            {
                base.AddOrder(order);
            }
        }

        protected override void RemoveOrder(KitchenShelf shelf, ShelfOrder order)
        {
            lock (_syncLock)
            {
                base.RemoveOrder(shelf, order);
            }
        }
        protected override void OnCourierArrival(ShelfOrder shelfOrder, KitchenShelf shelf, Timer timer)
        {
            lock (_syncLock)
            {
                base.OnCourierArrival(shelfOrder, shelf, timer);
            }
        }

        protected override void OnOrderDeterrioration(KitchenShelf shelf, ShelfOrder shelfOrder)
        {
            lock (_syncLock)
            {
                base.OnOrderDeterrioration(shelf, shelfOrder);
            }
        }
    }
}
