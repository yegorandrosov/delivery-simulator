using DeliverySimulator.Shared.Models;
using System.Collections.Generic;

namespace DeliverySimulator.OrderEmitter.OrderProviders
{
    /// <summary>
    /// Interface for the order provider services
    /// </summary>
    public interface IOrderProvider
    {
        /// <summary>
        /// Get orders from given provider
        /// </summary>
        /// <returns><see cref="Order"/></returns>
        List<Order> GetOrders();
    }
}
