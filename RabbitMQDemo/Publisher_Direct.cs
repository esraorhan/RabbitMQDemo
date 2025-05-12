using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public static class Publisher_Direct
    {
        public static void Send(string severity, string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // 📦 Direct exchange tanımı (fanout, topic değil!)
            channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

            var body = Encoding.UTF8.GetBytes(message);

            // 🎯 RoutingKey: error, info, warning...
            channel.BasicPublish(exchange: "direct_logs",
                                 routingKey: severity,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($"📤 [{severity}] mesaj gönderildi: {message}");
        }
    }
}
