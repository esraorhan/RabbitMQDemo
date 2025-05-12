using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public static class Publisher_Topic
    {
        public static void Send(string routingKey, string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // 🧭 Topic exchange
            channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "topic_logs",
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($"📤 Gönderildi: [{routingKey}] → {message}");
        }
    }
}
