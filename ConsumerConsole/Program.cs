using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ConsumerConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Mesaj bekleniyor...");
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("https://customer.cloudamqp.com/login")
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("inventiv-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);

                //channel.BasicAck(e.DeliveryTag, false);
            };

            channel.BasicConsume("inventiv-queue", true, consumer);

            //Console.WriteLine("ok.");
            Console.ReadLine();
        }
    }
}
