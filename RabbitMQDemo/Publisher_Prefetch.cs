using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public static class Publisher_Prefetch
    {
        public static void SendBatch(int count)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // 🔧 Kuyruk kalıcı (durable)
            channel.QueueDeclare("prefetch-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var props = channel.CreateBasicProperties();
            props.Persistent = true; // 🧱 Mesajlar da kalıcı olsun

            for (int i = 1; i <= count; i++)
            {
                string message = $"Prefetch Mesaj {i}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "prefetch-queue",
                                     basicProperties: props,
                                     body: body);

                Console.WriteLine($"📤 Gönderildi: {message}");
            }
        }
    }
}
