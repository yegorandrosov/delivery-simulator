using DeliverySimulator.Kitchen.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DeliverySimulator.Kitchen.Renderers
{
    /// <summary>
    /// Timer-based wrapper for <see cref="IRenderer"/>
    /// </summary>
    public class SimpleTimerRenderEngine : IRenderEngine, IDisposable
    {
        private readonly IRenderer renderer;
        private readonly Timer timer;

        /// <param name="renderer">Renderer to be used for rendering frames</param>
        /// <param name="fps">Number of frames per second</param>
        public SimpleTimerRenderEngine(IRenderer renderer, int fps = 10)
        {
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            if (fps <= 0)
            {
                throw new ArgumentException("Invalid fps value", nameof(fps));
            }
            timer = new Timer(1000 / fps);

            timer.Elapsed += (sender, args) =>
            {
                renderer.Render();
            };
        }

        /// <summary>
        /// Dispose all related resources
        /// </summary>
        public void Dispose()
        {
            timer.Dispose();
        }

        /// <summary>
        /// Start rendering process with preset frame rate
        /// </summary>
        public void Start()
        {
            timer.Start();
        }
    }
}
