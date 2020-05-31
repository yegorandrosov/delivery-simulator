using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.Shared
{
    /// <summary>
    /// Single queue RabbitMQ consumer
    /// </summary>
    public class QueueConsumer : IDisposable
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly EventingBasicConsumer consumer;

        /// <summary>
        /// Set up RabbitMQ consumer for specific queue
        /// </summary>
        /// <param name="queueName">Queue name</param>
        public QueueConsumer(string queueName)
        {
            var factory = new ConnectionFactory() { HostName = Configuration.RabbitMQ.Host };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            consumer.Received += (model, ea) =>
            {
                this.Received?.Invoke(model, ea);
            };
        }

        /// <summary>
        /// Triggered when new new message arrives in the queue.
        /// </summary>
        public event EventHandler<BasicDeliverEventArgs> Received;

        /// <summary>
        /// Release all related resources.
        /// </summary>
        public void Dispose()
        {
            connection?.Dispose();
            channel?.Dispose();
        }
    }
}
