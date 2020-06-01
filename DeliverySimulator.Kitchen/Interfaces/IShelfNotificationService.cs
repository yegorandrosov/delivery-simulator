using DeliverySimulator.Kitchen.Models;
using DeliverySimulator.Shared.Models;

namespace DeliverySimulator.Kitchen.Interfaces
{
    public interface IShelfNotificationService
    {
        /// <summary>
        /// Should be invoked when new order arrives in the kitchen
        /// </summary>
        /// <param name="shelf">Represents shelf order was placed into</param>
        /// <param name="order">Kitchen order</param>
        void PublishOrderReceivedEvent(KitchenShelf shelf, Order order);

        /// <summary>
        /// Should be invoked when order is picked up by a courier
        /// </summary>
        /// <param name="order">Order to be picked</param>
        /// <param name="elapsedTime">Elapsed time in seconds from receiving order to the pick up event</param>
        void PublishOrderReceivedByCourierEvent(Order order, int elapsedTime);

        /// <summary>
        /// When "overflow" shelf is full, random item should be discarded from it to reserve place for a new item.
        /// </summary>
        /// <param name="order">Order that is going to waste.</param>
        void PublishOrderDiscardedFromOverflowShelfEvent(Order order);

        /// <summary>
        /// Each order slowly loses its value, and when value goes to 0, it is not going to be picked anymore
        /// </summary>
        /// <param name="order">Order that is going to waste.</param>
        void PublishOrderDeterrioratedEvent(Order order);
    }
}
