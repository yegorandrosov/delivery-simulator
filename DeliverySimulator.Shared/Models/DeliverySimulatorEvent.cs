using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.Shared.Models
{
    public class DeliverySimulatorEvent
    {
        public string Message { get; set; }

        public DeliverySimulaterEventType Type { get; set; }
    }

    public enum DeliverySimulaterEventType
    {
        Neutral,
        Success,
        Fail,
    }
}
