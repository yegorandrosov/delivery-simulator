using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.Shared.Models
{
    /// <summary>
    /// Order data transfer object to exchange between workers
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Order Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Dish name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Dish temperature
        /// </summary>
        public string Temp { get; set; }

        /// <summary>
        /// Max time for item to remain in shelf awaiting for courier. When exceeded, item should be removed.
        /// </summary>
        public int ShelfLife { get; set; }


        public double DecayRate { get; set; }
    }
}
