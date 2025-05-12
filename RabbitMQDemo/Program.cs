// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQDemo;
using System.Text;

Console.WriteLine("🔄 RabbitMQ bağlantısı kuruluyor...");

Console.WriteLine("1 → Mesaj Gönder (Publisher)");
Console.WriteLine("2 → Mesaj Dinle (Consumer)");
Console.Write("Seçimin: ");
var input = Console.ReadLine();

if (input == "1")
{
    Console.Write("Mesaj: ");
    var message = Console.ReadLine();
    Publisher.SendMessage("hello-queue", message);
}
else if (input == "2")
{
    Consumer.StartListening("hello-queue");
}
else
{
    Console.WriteLine("❌ Geçersiz seçim.");
}