using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public static class Consumer_Prefetch2
    {
        public static void Listen()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("prefetch-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            // 🎯 Prefetch = bu tüketici de aynı kuralı uygulasın
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"🐢 [SLOW] Alındı: {message}");

                Thread.Sleep(4000); // Yavaş tüketici

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume("prefetch-queue", autoAck: false, consumer: consumer);
            Console.WriteLine("🐢 SLOW Consumer çalışıyor... Enter’a bas çık.");
            Console.ReadLine();
        }
    }
}
