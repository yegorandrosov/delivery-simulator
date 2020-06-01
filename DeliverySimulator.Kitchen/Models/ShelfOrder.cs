using DeliverySimulator.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DeliverySimulator.Kitchen.Models
{
    /// <summary>
    /// Order wrapper extended with business logic. Stores references for timers that should trigger events related to this order.
    /// </summary>
    public class ShelfOrder
    {
        private readonly DateTime addedAt;
        private List<Timer> timers;

        public ShelfOrder(Order order, DateTime addedAt)
        {
            Order = order;
            this.addedAt = addedAt;
            timers = new List<Timer>();
        }

        public Order Order { get; }

        private bool _hasTriggeredEvent;
        public bool HasTriggeredEvent
        {
            get
            {
                return _hasTriggeredEvent;
            }
            set
            {
                _hasTriggeredEvent = true;

                StopAllTimers();
            }
        }

        /// <summary>
        /// Get current deterrioration value. When reaches 0, order should be moved to the waste
        /// </summary>
        /// <param name="currentTime">Time snapshot to calculate order age</param>
        /// <param name="shelfDecayModifier">Shelf constant that affects deterrioration speed. Overflow shelf usually has higher rate.</param>
        /// <returns>double in range from 0 to 1</returns>
        public double GetValue(DateTime currentTime, int shelfDecayModifier)
        {
            var orderAge = currentTime - addedAt;

            if (Order.ShelfLife == 0)
            {
                return 0;
            }

            return (Order.ShelfLife - Order.DecayRate * orderAge.TotalSeconds * shelfDecayModifier) / Order.ShelfLife;
        }

        /// <summary>
        /// Transformed GetValue formula to get OrderAge at which GetValue will return 0
        /// </summary>
        /// <param name="shelfDecayModifier">Shelf decay modifier. Overflow shelf has greater decay modifier than a regular one.</param>
        /// <returns>Time in seconds when order will be deterriorated and should be thrown away.</returns>
        public double GetMaxOrderAge(int shelfDecayModifier)
        {
            return Order.ShelfLife / (Order.DecayRate * shelfDecayModifier);
        }

        /// <summary>
        /// Add timer to inner collection to store reference.
        /// </summary>
        /// <param name="timer"></param>
        public void AddTimer(Timer timer)
        {
            timers.Add(timer);
        }

        /// <summary>
        /// Cancells all events. Should be invoked when any timer hits Elapsed event.
        /// </summary>
        public void StopAllTimers()
        {
            foreach (var timer in timers)
            {
                timer.Stop();
                timer.Dispose();
            }
        }
    }
}
