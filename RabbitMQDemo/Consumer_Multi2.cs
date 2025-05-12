using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public static class Consumer_Multi2
    {
        public static void Listen()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("multi-consumer-queue", true, false, false, null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"[Consumer 2] 📥 {message}");
                Thread.Sleep(1500);
                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume("multi-consumer-queue", autoAck: false, consumer: consumer);
            Console.WriteLine("⏳ Consumer 2 dinliyor... Enter’a bas çık.");
            Console.ReadLine();
        }
    }
}
