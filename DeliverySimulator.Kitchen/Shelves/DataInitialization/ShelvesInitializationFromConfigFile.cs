using DeliverySimulator.Kitchen.Interfaces;
using DeliverySimulator.Kitchen.Models;
using DeliverySimulator.Shared;
using System.Collections.Generic;

namespace DeliverySimulator.Kitchen.Shelves.DataInitialization
{
    public class ShelvesInitializationFromConfigFile : IShelvesInitialization
    {
        public IEnumerable<KitchenShelf> GetShelves()
        {
            foreach (var defaultShelf in AppSettings.Instance.AppConfig.Shelves.DefaultShelves)
            {
                yield return new KitchenShelf(defaultShelf.Name, defaultShelf.Capacity, defaultShelf.DecayModifier);
            }
        }
    }
}
