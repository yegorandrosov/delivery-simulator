using DeliverySimulator.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.OrderEmitter.OrderProviders
{
    /// <summary>
    /// JSON text input based order provider
    /// </summary>
    public class JsonOrderProvider : IOrderProvider
    {
        private readonly string json;
        private readonly List<Order> orders;

        /// <summary>
        /// Construct new instance of <see cref="JsonOrderProvider"/>
        /// </summary>
        /// <param name="json">JSON input string to parse</param>
        public JsonOrderProvider(string json)
        {
            json = json ?? throw new ArgumentNullException(nameof(json));

            this.json = json;
            this.orders = JsonConvert.DeserializeObject<List<Order>>(json);
        }

        /// <summary>
        /// Get orders deserialized from JSON input
        /// </summary>
        /// <returns><see cref="Order"/></returns>
        public List<Order> GetOrders()
        {
            return orders;
        }
    }
}
