using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public static class Publisher_Fanout
    {
        public static void Broadcast(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // 🔊 Fanout Exchange oluştur
            channel.ExchangeDeclare(exchange: "logs_fanout", type: ExchangeType.Fanout);

            var body = Encoding.UTF8.GetBytes(message);

            // 🔥 Routing key yok! Herkese gider
            channel.BasicPublish(exchange: "logs_fanout",
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($"📣 Yayınlandı (fanout): {message}");
        }
    }
}
