using DeliverySimulator.Kitchen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.Kitchen.Shelves
{
    public interface IKitchenShelfFactory
    {
        KitchenShelf Create(string temp);
    }
}
