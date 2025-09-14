using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

const string message = "Fanout message from dot net application";
var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(
    exchange: "amq.fanout",
    routingKey: string.Empty,
    basicProperties: null,
    body: body
);

Console.WriteLine($" [x] Sent {message}");
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
