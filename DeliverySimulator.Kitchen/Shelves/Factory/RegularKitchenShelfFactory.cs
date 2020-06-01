using DeliverySimulator.Kitchen.Models;
using DeliverySimulator.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.Kitchen.Shelves
{
    /// <summary>
    /// <see cref="IKitchenShelfFactory"/> implementation for creating <see cref="KitchenShelf"/> with constant capacity and decay modifier
    /// </summary>
    public class RegularKitchenShelfFactory : IKitchenShelfFactory
    {
        /// <summary>
        /// Create new instance of <see cref="KitchenShelf" /> with constant capacity and decay modifier
        /// </summary>
        /// <param name="temp">temperature identifier of the shelf</param>
        /// <returns></returns>
        public KitchenShelf Create(string temp)
        {
            return new KitchenShelf(temp, AppSettings.Instance.AppConfig.Shelves.RegularShelfCapacity, AppSettings.Instance.AppConfig.Shelves.RegularShelfDecayModifier);
        }
    }
}
