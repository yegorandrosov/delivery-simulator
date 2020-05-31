using DeliverySimulator.Kitchen.Shelves;
using DeliverySimulator.Kitchen.Shelves.Notifications;
using DeliverySimulator.Kitchen.Shelves.ShelfTimers;
using DeliverySimulator.Shared;
using DeliverySimulator.Shared.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DeliverySimulator.Kitchen.Models
{
    /// <summary>
    /// Kitchen order processor: receives orders, sets up events for them, sends notifications for internal events
    /// </summary>
    public class KitchenShelvesManager : IEnumerable<KitchenShelf>
    {
        private List<KitchenShelf> shelves;
        private readonly IKitchenShelfFactory kitchenShelfFactory;
        private readonly IOrderTimerFactory courierTimerFactory;
        private readonly IOrderTimerFactory deterriorationTimerFactory;
        private readonly IShelfNotificationService shelfNotificationService;

        /// <summary>
        /// Create new instance of <see cref="KitchenShelvesManager"/>
        /// </summary>
        /// <param name="kitchenShelfFactory">Factory to create missing shelves when order with new <see cref="Order.Temp"/> arrives</param>
        /// <param name="courierTimerFactory">Timer factory to initialize courier routine</param>
        /// <param name="deterriorationTimerFactory">Timer factory to initialize deterrioration timers for orders</param>
        /// <param name="shelfNotificationService">Notification service to send messages about internal events</param>
        public KitchenShelvesManager(
            IKitchenShelfFactory kitchenShelfFactory, 
            IOrderTimerFactory courierTimerFactory,
            IOrderTimerFactory deterriorationTimerFactory,
            IShelfNotificationService shelfNotificationService)
        {
            shelves = new List<KitchenShelf>();

            Initialize();
            this.kitchenShelfFactory = kitchenShelfFactory;
            this.courierTimerFactory = courierTimerFactory;
            this.deterriorationTimerFactory = deterriorationTimerFactory;
            this.shelfNotificationService = shelfNotificationService;
        }

        /// <summary>
        /// Add "overflow" shelf to the storage.
        /// </summary>
        protected virtual void Initialize()
        {
            shelves.Add(new KitchenShelf(Configuration.Shelves.OverflowShelfName,
                Configuration.Shelves.OveflowShelfCapacity,
                Configuration.Shelves.OverflowShelfDecayModifier)
            );
        }

        /// <summary>
        /// Add order to shelves. Sets up 'courier pickup' and 'deterrioration' events.
        /// </summary>
        /// <param name="order">Order to add.</param>
        public virtual void AddOrder(Order order)
        {
            var shelf = shelves.FirstOrDefault(x => x.Temp == order.Temp);

            if (shelf == null)
            {
                shelf = kitchenShelfFactory.Create(order.Temp);
                shelves.Add(shelf);
            }

            var shelfOrder = new ShelfOrder(order, DateTime.Now);
            if (!shelf.Add(shelfOrder))
            {
                shelf = AddToOverflowShelf(shelfOrder);
            }

            SetUpCourierTimer(shelf, shelfOrder);
            SetUpDeterriorationTimer(shelf, shelfOrder);
            shelfNotificationService.PublishOrderReceivedEvent(shelf, order);

        }

        protected virtual void SetUpDeterriorationTimer(KitchenShelf shelf, ShelfOrder shelfOrder)
        {
            var timer = deterriorationTimerFactory.Create(shelf, shelfOrder);

            timer.Elapsed += (sender, args) =>
            {
                OnOrderDeterrioration(shelf, shelfOrder);
            };

            timer.Start();

            shelfOrder.AddTimer(timer);
        }

        protected virtual void OnOrderDeterrioration(KitchenShelf shelf, ShelfOrder shelfOrder)
        {
            shelfOrder.StopAllTimers();
            RemoveOrder(shelf, shelfOrder);
            shelfNotificationService.PublishOrderDeterrioratedEvent(shelfOrder.Order);
        }

        protected virtual KitchenShelf AddToOverflowShelf(ShelfOrder shelfOrder)
        {
            var shelf = shelves.FirstOrDefault(x => x.Temp == Configuration.Shelves.OverflowShelfName);

            if (shelf.Orders.Count == shelf.MaxCapacity)
            {
                var removedShelfOrder = shelf.RemoveRandom();
                removedShelfOrder.StopAllTimers();
                shelfNotificationService.PublishOrderDiscardedFromOverflowShelfEvent(shelfOrder.Order);
            }
            
            shelf.Add(shelfOrder);

            return shelf;
        }

        protected virtual void RemoveOrder(KitchenShelf shelf, ShelfOrder order)
        {
            shelf.Remove(order);
        }

        protected virtual void SetUpCourierTimer(KitchenShelf shelf, ShelfOrder shelfOrder)
        {
            var timer = courierTimerFactory.Create(shelf, shelfOrder);

            timer.Elapsed += (sender, args) =>
            {
                OnCourierArrival(shelfOrder, shelf, timer);
            };

            timer.Start();

            shelfOrder.AddTimer(timer);
        }

        protected virtual void OnCourierArrival(ShelfOrder shelfOrder, KitchenShelf shelf, Timer timer)
        {
            shelfOrder.StopAllTimers();
            shelf.Remove(shelfOrder);
            shelfNotificationService.PublishOrderReceivedByCourierEvent(shelfOrder.Order, (int)timer.Interval / 1000);
        }

        public IEnumerator<KitchenShelf> GetEnumerator()
        {
            return shelves.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
