// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQDemo;
using System.Text;

Console.WriteLine("🐰 RabbitMQ Test Menü:");
Console.WriteLine("1 → Basic Mesaj Gönder (hello-queue)");
Console.WriteLine("2 → Basic Mesaj Dinle");

Console.WriteLine("3 → Kalıcı Mesaj Gönder (durable-queue)");
Console.WriteLine("4 → Kalıcı Mesaj Dinle");

Console.WriteLine("5 → Manual ACK Gönder (manual-ack-queue)");
Console.WriteLine("6 → Manual ACK Dinle");

Console.WriteLine("7 → Multiple Consumer Testi (GELİYOR)");
Console.WriteLine("8 → Prefetch (Fair Dispatch) Testi (GELİYOR)");

Console.WriteLine("9 → prefetch Gönder (GELİYOR)");
Console.WriteLine("10 → prefetch Dinle (GELİYOR)");

Console.WriteLine("11 → Fanout Exchange Gönder (GELİYOR)");
Console.WriteLine("12 → Fanout Exchange Dinle (GELİYOR)");

Console.WriteLine("13 → FANOUT EXCHANGE Gönder (GELİYOR)");
Console.WriteLine("14 → FANOUT EXCHANGE Dinle (GELİYOR)");

Console.WriteLine("0 → Çıkış");

Console.Write("Seçiminiz: ");
var secim = Console.ReadLine();

switch (secim)
{
    case "1":
        Console.Write("Mesaj gir: ");
        var msg1 = Console.ReadLine();
        Publisher.SendMessage("hello-queue", msg1);
        break;

    case "2":
        Consumer.StartListening("hello-queue");
        break;

    case "3":
        Console.Write("Kalıcı mesaj gir: ");
        var msg2 = Console.ReadLine();
        Publisher.SendMessage("hello-queue", msg2);
        break;

    case "4":
        Consumer.StartListening("hello-queue");
        break;

    case "5":
        Console.Write("Manual ack mesaj gir: ");
        var msg3 = Console.ReadLine();
        Publisher_ManualAck.Send(msg3);
        break;

    case "6":
        Consumer_ManualAck.Listen();
        break;

    case "7":
        Console.Write("Kaç mesaj gönderilsin?: ");
        var adet = int.Parse(Console.ReadLine());
        Publisher_MultiConsumer.SendBatchMessages(adet);
        break;

    case "8":
        Consumer_Multi2.Listen();
        Consumer_Multi1.Listen();
        Console.WriteLine("Consumer 1 → ayrı terminalde: Consumer_Multi1.Listen();");
        Console.WriteLine("Consumer 2 → ayrı terminalde: Consumer_Multi2.Listen();");
        Console.WriteLine("📌 Bunlar .NET Core App olarak 2 terminalde ayrı ayrı çalıştırılmalı.");
        break;

    case "9":
        Console.Write("Kaç mesaj gönderilsin?: ");
        var adet9 = int.Parse(Console.ReadLine());
        Publisher_Prefetch.SendBatch(adet9);
        break;
    case "10":
        Consumer_Prefetch1.Listen();
        Consumer_Prefetch2.Listen();
        Console.WriteLine("Ayrı terminalde çalıştır: Consumer_Prefetch1.Listen();");
        Console.WriteLine("Başka terminalde çalıştır: Consumer_Prefetch2.Listen();");
        break;
    case "11":
        Console.Write("Log seviyesi (info / error): ");
        var sev = Console.ReadLine();
        Console.Write("Mesaj: ");
        var msg11 = Console.ReadLine();
        Publisher_Direct.Send(sev, msg11);
        break;
    case "12":
        Console.WriteLine("İki consumer başlatılıyor...");

        var infoThread = new Thread(() => Consumer_Info.Listen());
        var errorThread = new Thread(() => Consumer_Error.Listen());

        infoThread.Start();
        errorThread.Start();

        Console.WriteLine("⏳ Her iki consumer da çalışıyor. Enter’a basmak çıkışa neden olur.");
        Console.ReadLine(); // program açık kalsın
        break;
    case "13":
        Console.Write("Mesajı gir (herkese yayılacak): ");
        var msg13 = Console.ReadLine();
        Publisher_Fanout.Broadcast(msg13);
        break;
    case "14":

        Console.WriteLine("Consumer_Fanout1 ve Consumer_Fanout2 ayrı çalıştırılmalı.");
        break;

    case "0":
        Console.WriteLine("👋 Görüşmek üzere!");
        break;

    default:
        Console.WriteLine("❌ Geçersiz seçim.");
        break;
}

