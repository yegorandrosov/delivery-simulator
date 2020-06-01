using DeliverySimulator.Kitchen.Models;
using System.Collections.Generic;

namespace DeliverySimulator.Kitchen.Interfaces
{
    public interface IShelvesInitialization
    {
        IEnumerable<KitchenShelf> GetShelves();
    }
}
