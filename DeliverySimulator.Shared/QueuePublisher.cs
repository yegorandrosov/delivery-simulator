using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.Shared
{
    public class QueuePublisher : IDisposable
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string queueName;

        public QueuePublisher(string queueName)
        {
            var factory = new ConnectionFactory() { HostName = AppSettings.Instance.AppConfig.RabbitMQ.Host };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            this.queueName = queueName;
        }

        public void Publish<T>(T item)
        {
            var message = JsonConvert.SerializeObject(item);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: null,
                                 body: body);

            Published?.Invoke(this, new QueuePublisherEventArgs(message));
        }

        public event EventHandler<QueuePublisherEventArgs> Published;

        public void Dispose()
        {
            connection?.Dispose();
            channel?.Dispose();
        }
    }

    public class QueuePublisherEventArgs : EventArgs
    {
        public QueuePublisherEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
