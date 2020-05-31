using System;
using System.Collections.Generic;
using System.IO;
using DeliverySimulator.Shared.Models;

namespace DeliverySimulator.OrderEmitter.OrderProviders
{
    /// <summary>
    /// JSON file input based order provider
    /// </summary>
    public class JsonFileOrderProvider : IOrderProvider
    {
        private IOrderProvider jsonOrderProvider;

        /// <summary>
        ///  Construct new instance of <see cref="JsonFileOrderProvider"/>
        /// </summary>
        /// <param name="filePath">Full path to json file</param>
        public JsonFileOrderProvider(string filePath)
        {
            filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));

            var fileContent = File.ReadAllText(filePath);
            jsonOrderProvider = new JsonOrderProvider(fileContent);
        }

        /// <summary>
        /// Get orders from json file
        /// </summary>
        public List<Order> GetOrders()
        {
            return jsonOrderProvider.GetOrders();
        }
    }
}
