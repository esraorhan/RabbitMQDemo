using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public static class Consumer
    {
        public static void StartListening(string queueName)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Producer'daki kuyrukla birebir aynı özelliklerde tanımlanmalı
            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            // 1️⃣ Mesajlar geldiğinde tetiklenecek olay sınıfı
            var consumer = new EventingBasicConsumer(channel);

            // 2️⃣ Mesaj geldiğinde bu olay devreye girer
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray(); // byte[] olarak gelen mesaj
                var message = Encoding.UTF8.GetString(body); // string'e çeviriyoruz

                Console.WriteLine($"📥 Mesaj alındı: {message}");
            };
            // 3️⃣ Kuyruğu dinlemeye başlıyoruz
            channel.BasicConsume(queue: queueName,
                           autoAck: true, // mesaj geldiğinde otomatik işlendi kabul et
                           consumer: consumer);

            Console.WriteLine("⏳ Dinleniyor... Çıkmak için Enter'a bas.");
            Console.ReadLine();
        }
    }
}
