using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public static class Consumer_Error
    {
        public static void Listen()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);
            channel.QueueDeclare(queue: "error-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            // 🎯 error tipli mesajları alır
            channel.QueueBind(queue: "error-queue", exchange: "direct_logs", routingKey: "error");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"[ERROR] Alındı: {message}");
            };

            channel.BasicConsume(queue: "error-queue", autoAck: true, consumer: consumer);
            Console.WriteLine("🎧 ERROR dinleniyor... Enter’a bas çık.");
            Console.ReadLine();
        }
    }
}
