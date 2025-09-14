using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

Console.WriteLine(" [*] Waiting for messages.");

// Consume from multiple fanout queues
ConsumeMessage("q.fanout1");
ConsumeMessage("q.fanout2");
ConsumeMessage("q.fanout3");
ConsumeMessage("q.fanout4");
ConsumeMessage("q.fanout5");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

// Reusable method
void ConsumeMessage(string queue)
{
    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        // Process the message
        Console.WriteLine($" [x] Received {message} - from queue {queue}");
    };

    channel.BasicConsume(
        queue: queue,
        autoAck: true,
        consumer: consumer
    );
}
