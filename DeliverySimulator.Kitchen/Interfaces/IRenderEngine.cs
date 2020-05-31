using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.Kitchen.Interfaces
{
    /// <summary>
    /// Render engine inteface. See <see cref="DeliverySimulator.Kitchen.Renderers.SimpleTimerRenderEngine"/>
    /// </summary>
    interface IRenderEngine
    {
        /// <summary>
        /// Start rendering process.
        /// </summary>
        void Start();
    }
}
