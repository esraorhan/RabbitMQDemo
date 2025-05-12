using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public static class Consumer_Topic_User
    {
        public static void Listen()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);

            var queueName = "user-topic-queue";
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            // user.# = user.created, user.profile.update gibi her seviyeyi alır
            channel.QueueBind(queue: queueName, exchange: "topic_logs", routingKey: "user.#");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var msg = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"[USER.#] Alındı: {ea.RoutingKey} → {msg}");
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            Console.WriteLine("🎧 USER.# dinleniyor...");
            Console.ReadLine();
        }
    }

}
