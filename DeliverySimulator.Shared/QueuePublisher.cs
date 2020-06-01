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
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private readonly string queueName;

        public QueuePublisher(string queueName)
        {
            _factory = new ConnectionFactory() { HostName = AppSettings.Instance.AppConfig.RabbitMQ.Host };
            this.queueName = queueName;
        }

        public void Publish<T>(T item)
        {
            if (_connection == null || !_connection.IsOpen)
                _connection = _factory.CreateConnection();

            using (IModel channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                var message = JsonConvert.SerializeObject(item);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);
                Published?.Invoke(this, new QueuePublisherEventArgs(message));
            }

        }

        public event EventHandler<QueuePublisherEventArgs> Published;

        public void Dispose()
        {
            _connection?.Dispose();
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
