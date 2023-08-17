using RabbitMailSender.API.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { Uri = new Uri("amqps://ceahuxxx:0wx2TxaTAA_fp_PzC9qdJfBFR23p87z5@woodpecker.rmq.cloudamqp.com/ceahuxxx") };

        var connection = factory.CreateConnection();

        using var channel = connection.CreateModel();

        channel.QueueDeclare("mailQueue", false, false, false);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var mailSender = JsonSerializer.Deserialize<Mail>(message);

            Console.WriteLine("Received Mail:");
            Console.WriteLine("Subject: {0}", mailSender.Subject);
            Console.WriteLine("Message: {0}", mailSender.Message);
            Console.WriteLine("From: {0}", mailSender.From);
            Console.WriteLine("To: {0}", mailSender.To);

        };

        channel.BasicConsume(queue: "mail_queue",
                                    autoAck: true,
                                    consumer: consumer);

        Console.WriteLine("Press [Enter] to exit.");
        Console.ReadLine();

    }
}