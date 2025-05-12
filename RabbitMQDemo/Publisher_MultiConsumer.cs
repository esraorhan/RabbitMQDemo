using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public static class Publisher_MultiConsumer
    {
        public static void SendBatchMessages(int count)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("multi-consumer-queue", true, false, false, null);

            var props = channel.CreateBasicProperties();
            props.Persistent = true;

            for (int i = 1; i <= count; i++)
            {
                string message = $"Mesaj {i}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "multi-consumer-queue",
                                     basicProperties: props,
                                     body: body);

                Console.WriteLine($"📤 Gönderildi: {message}");
            }
        }
    }
}
