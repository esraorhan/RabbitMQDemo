using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public static class Consumer_Topic_Log
    {
        public static void Listen()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);

            var queueName = "log-topic-queue";
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            // log.* = log.info, log.error gibi tek seviyeyi alır
            channel.QueueBind(queue: queueName, exchange: "topic_logs", routingKey: "log.*");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var msg = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"[LOG.*] Alındı: {ea.RoutingKey} → {msg}");
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            Console.WriteLine("🎧 LOG.* dinleniyor...");
            Console.ReadLine();
        }
    }
}
