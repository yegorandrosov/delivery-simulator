using DeliverySimulator.OrderEmitter.OrderProviders;
using DeliverySimulator.Shared;
using DeliverySimulator.Shared.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DeliverySimulator.OrderEmitter
{
    /// <summary>
    /// Emits orders as messages to RabbitMQ publisher
    /// </summary>
    public class EmitOrdersService
    {
        private readonly QueuePublisher publisher;
        private readonly IOrderProvider orderProvider;
        private Stack<Order> orderStack;
        private Timer timer;
        private bool isOutOfOrdersTriggered;
        private object _lock = new object();

        public delegate void OutOfOrdersEventHandler(object sender, EventArgs args);
        public event OutOfOrdersEventHandler OnOutOfOrders;

        /// <summary>
        /// Create new instance of <see cref="EmitOrdersService"/>
        /// </summary>
        /// <param name="publisher">RabbitMQ single queue publisher</param>
        public EmitOrdersService(QueuePublisher publisher, IOrderProvider orderProvider)
        {
            this.publisher = publisher;
            this.orderProvider = orderProvider;
        }

        /// <summary>
        /// Start timer-based order emitting process
        /// </summary>
        public void StartEmittingOrders()
        {
            ReinitializeComponent();

            var orders = orderProvider.GetOrders();
            orderStack = new Stack<Order>(orders);

            timer.Elapsed += new ElapsedEventHandler((sender, args) =>
            {
                try
                {
                    lock (_lock)
                    {
                        if (orderStack.Count == 0)
                        {
                            if (!isOutOfOrdersTriggered)
                            {
                                isOutOfOrdersTriggered = true;
                                timer.Dispose();  
                                OnOutOfOrders?.Invoke(this, null);
                            }

                            timer.Dispose();
                            return;
                        }
                        else
                        {
                            var order = orderStack.Pop();

                            publisher.Publish(order);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }
            });

            timer.Start();
        }

        private void ReinitializeComponent()
        {
            timer?.Dispose();
            timer = new Timer();
            timer.Interval = 1000 / Configuration.OrderEmitter.NumberOfOrdersPerSecond;
            isOutOfOrdersTriggered = false;
        }
    }
}
