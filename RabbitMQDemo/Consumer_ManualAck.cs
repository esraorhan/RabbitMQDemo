using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public static class Consumer_ManualAck
    {
        public static void Listen()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("manual-ack-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"📥 Alındı: {message}");

                Console.WriteLine("⏳ İşlem simülasyonu...");
                Thread.Sleep(2000); // işleniyormuş gibi beklet

                // ✋ El ile onay veriyoruz — işledik, silebilirsin diyoruz
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                Console.WriteLine("✅ ACK gönderildi. Mesaj silindi.");
            };

            // 🔥 autoAck = false → mesajı hemen silme, biz söyleyince sil
            channel.BasicConsume(queue: "manual-ack-queue", autoAck: false, consumer: consumer);

            Console.WriteLine("⏳ Dinleniyor (manual ack). Enter'a bas çık.");
            Console.ReadLine();
        }
    }
}
