using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public static class Consumer_Fanout1
    {
        public static void Listen()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "logs_fanout", type: ExchangeType.Fanout);

            // 🔧 Unique queue tanımla (ya sabit isim ya random)
            string queueName = "fanout-queue-1";
            channel.QueueDeclare(queue: queueName,
                durable: false, 
                exclusive: false, 
                autoDelete: false, 
                arguments: null);

            // 🎯 Bu kuyruğu exchange'e bağla
            channel.QueueBind(queue: queueName, exchange: "logs_fanout", routingKey: "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"[FANOUT 1] Alındı: {message}");
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            Console.WriteLine("📻 FANOUT Consumer 1 dinliyor...");
            Console.ReadLine();
        }
    }
}
