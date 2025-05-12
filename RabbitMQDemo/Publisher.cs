using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo
{
    public static class Publisher
    {
        public static void SendMessage(string queueName, string message)
        {
            // 1️⃣ RabbitMQ ile bağlantı için "ConnectionFactory" oluşturuyoruz
            var factory = new ConnectionFactory() { HostName = "localhost" };
            // 2️⃣ Sunucuya bağlanıyoruz (yani TCP bağlantısı açılıyor)
            using var connection = factory.CreateConnection();
            // 3️⃣ Bağlantıdan bir "kanal" (channel) oluşturuyoruz
            // Channel, RabbitMQ'da işlem yapacağımız hattır
            using var channel = connection.CreateModel();

            // 4️⃣ Mesaj göndereceğimiz kuyruğu tanımlıyoruz
            // Eğer bu kuyruk daha önce tanımlandıysa dokunulmaz, sadece mevcut olan kullanılır
            channel.QueueDeclare(queue: queueName,
                             durable: false,        // mesajlar diske yazılmasın (kapanınca silinir)
                             exclusive: false,      // sadece bu bağlantı mı kullanır? Hayır
                             autoDelete: false,     // kuyruğa kimse bağlı değilse otomatik silinsin mi? Hayır
                             arguments: null);      // ek ayar yok
         
            // 5️⃣ Mesajı byte dizisine (byte[]) çeviriyoruz çünkü RabbitMQ byte alır
            var body =Encoding.UTF8.GetBytes(message);

            // 6️⃣ Mesajı kuyruğa gönderiyoruz
            channel.BasicPublish(exchange: "",          // boş = default exchange
                             routingKey: queueName, // hangi kuyruğa gidecek?
                             basicProperties: null, // ekstra başlık vs yok
                             body: body);

            Console.WriteLine($"📤 Mesaj gönderildi: {message}");

        }
    }
}
