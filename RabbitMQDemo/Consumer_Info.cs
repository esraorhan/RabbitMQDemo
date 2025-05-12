using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public static class Consumer_Info
    {
        public static void Listen()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

            // 🔧 Özel queue oluşturulur
            channel.QueueDeclare(queue: "info-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            // 🎯 Sadece routingKey = "info" olan mesajları al
            channel.QueueBind(queue: "info-queue", exchange: "direct_logs", routingKey: "info");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"[INFO] Alındı: {message}");
            };

            channel.BasicConsume(queue: "info-queue", autoAck: true, consumer: consumer);
            Console.WriteLine("🎧 INFO dinleniyor... Enter’a bas çık.");
            Console.ReadLine();
        }
    }
}
