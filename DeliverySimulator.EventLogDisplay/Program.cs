using DeliverySimulator.Shared;
using DeliverySimulator.Shared.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySimulator.EventLogDisplay
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var eventQueueConsumer = new QueueConsumer(Configuration.RabbitMQ.EventQueueName))
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

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
