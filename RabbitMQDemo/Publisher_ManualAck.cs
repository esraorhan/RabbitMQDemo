using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public static class Publisher_ManualAck
    {
        //Gönderici (kalıcı mesaj + kuyruk)
        public static void Send(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Kalıcı kuyruk
            channel.QueueDeclare("manual-ack-queue", 
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Kalıcı mesaj
            var props = channel.CreateBasicProperties();
            props.Persistent = true;

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: "manual-ack-queue",
                                 basicProperties: props,
                                 body: body);

            Console.WriteLine($"📤 Gönderildi (manual ack): {message}");
        }
    }
}
