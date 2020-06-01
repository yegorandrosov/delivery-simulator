using DeliverySimulator.Shared;
using DeliverySimulator.Shared.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeliverySimulator.EventLogDisplay
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var eventTerminationQueueConsumer = new QueueConsumer(AppSettings.Instance.AppConfig.RabbitMQ.EventLogDisplayTerminationQeueueName))
            using (var eventQueueConsumer = new QueueConsumer(AppSettings.Instance.AppConfig.RabbitMQ.EventQueueName))
            {
                eventQueueConsumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    var eventItem = JsonConvert.DeserializeObject<DeliverySimulatorEvent>(message);

                    if (eventItem.Type != DeliverySimulaterEventType.Neutral)
                    {
                        switch (eventItem.Type)
                        {
                            case DeliverySimulaterEventType.Fail:
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                break;
                            case DeliverySimulaterEventType.Success:
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                break;
                        }
                    }

                    Console.WriteLine("{0} Received {1}", DateTime.Now, eventItem.Message);

                    Console.ForegroundColor = ConsoleColor.White;
                };

                eventTerminationQueueConsumer.Received += (model, ea) =>
                {
                    Console.WriteLine("Termination was requested");

                    for (var i = 5; i != 0; i--)
                    {
                        Console.WriteLine($"Shutting down in {i}");
                        Thread.Sleep((int)TimeSpan.FromSeconds(1).TotalMilliseconds);
                    }

                    Environment.Exit(0);
                };

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
