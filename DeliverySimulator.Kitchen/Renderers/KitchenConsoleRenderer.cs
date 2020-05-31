using DeliverySimulator.Kitchen.Interfaces;
using DeliverySimulator.Kitchen.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliverySimulator.Kitchen.Renderers
{
    /// <summary>
    /// Simple console renderer to display content of shelves in the kitchen
    /// </summary>
    public class KitchenConsoleRenderer : IRenderer
    {
        private readonly IEnumerable<KitchenShelf> kitchenShelves;

        public KitchenConsoleRenderer(IEnumerable<KitchenShelf> kitchenShelves)
        {
            this.kitchenShelves = kitchenShelves;
        }

        /// <summary>
        /// Display list of shelves with content of each
        /// </summary>
        public void Render()
        {
            Console.Clear();
            DateTime timeSnapshot = DateTime.Now;

            foreach (var kitchenShelf in kitchenShelves)
            {
                Console.WriteLine($"Shelf \"{kitchenShelf.Temp}\"");

                if (!kitchenShelf.Orders.Any())
                {
                    Console.WriteLine("Shelf is empty.");
                }
                else
                {
                    var ind = 0;
                    foreach (var shelfOrder in kitchenShelf.Orders)
                    {
                        var orderValue = shelfOrder.GetValue(timeSnapshot, kitchenShelf.DecayModifier);

                        if (orderValue <= 0)
                        {
                            continue;
                        }

                        Console.WriteLine($"{++ind}. Order ID: {shelfOrder.Order.Id}. Name: {shelfOrder.Order.Name}. Current value: {orderValue}");
                    }
                }

                Console.WriteLine("====================================");
            }
        }
    }
}
