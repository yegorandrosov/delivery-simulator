using DeliverySimulator.Kitchen.Models;
using System.Timers;

namespace DeliverySimulator.Kitchen.Interfaces
{
    /// <summary>
    /// Factory interface to get timer for kitchen shelf events such as deterrioration or courier arrival
    /// </summary>
    public interface IOrderTimerFactory
    {
        /// <summary>
        /// Get timer for kitchen shelf event
        /// </summary>
        /// <param name="shelf">Shelf which contains the order. Can be ignored in some implementations</param>
        /// <param name="order">Order. Can be ignored in some implementations</param>
        /// <returns>Timer with calculated interval</returns>
        Timer Create(KitchenShelf shelf, ShelfOrder order);
    }
}
