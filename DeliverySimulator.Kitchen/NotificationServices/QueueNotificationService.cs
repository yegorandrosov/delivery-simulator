using DeliverySimulator.Kitchen.Models;
using DeliverySimulator.Kitchen.Shelves.Notifications;
using DeliverySimulator.Shared;
using DeliverySimulator.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.Kitchen.NotificationServices
{
    /// <summary>
    /// IShelfNotificationService implementation to work with RabbitMQ publisher
    /// </summary>
    public class QueueNotificationService : IShelfNotificationService
    {
        private readonly QueuePublisher queuePublisher;

        public QueueNotificationService(QueuePublisher queuePublisher)
        {
            this.queuePublisher = queuePublisher;
        }

        /// <inheritdoc/>
        public void PublishOrderDeterrioratedEvent(Order order)
        {
            queuePublisher.Publish(new DeliverySimulatorEvent()
            {
                Message = $"Order {order.Id} deterriorated.",
                Type = DeliverySimulaterEventType.Fail,
            });
        }

        /// <inheritdoc/>
        public void PublishOrderDiscardedFromOverflowShelfEvent(Order order)
        {
            queuePublisher.Publish(new DeliverySimulatorEvent()
            {
                Message = $"Overflow shelf is full. Order {order.Id} is going to waste.",
                Type = DeliverySimulaterEventType.Fail,
            });
        }

        /// <inheritdoc/>
        public void PublishOrderReceivedByCourierEvent(Order order, int elapsedTime)
        {
            queuePublisher.Publish(new DeliverySimulatorEvent()
            {
                Message = $"Order {order.Id} was picked up by courier after {elapsedTime} seconds.",
                Type = DeliverySimulaterEventType.Success,
            });
        }

        /// <inheritdoc/>
        public void PublishOrderReceivedEvent(KitchenShelf shelf, Order order)
        {
            queuePublisher.Publish(new DeliverySimulatorEvent()
            {
                Message = $"Order {order.Id} was cooked and placed into \"{shelf.Temp}\" shelf. Number of items in shelf - {shelf.Orders.Count} / {shelf.MaxCapacity}."
            });
        }
    }
}
