using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.Kitchen.Interfaces
{
    /// <summary>
    /// Interface for frame renderers. See <see cref="DeliverySimulator.Kitchen.Renderers.KitchenConsoleRenderer"/>
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Render frame
        /// </summary>
        void Render();
    }
}
