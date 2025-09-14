using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    //Here , do some processing .

    Console.WriteLine($" [x] Received {message}");
};

channel.BasicConsume(queue: "q.header1",
                     autoAck: true,
                     consumer: consumer);
channel.BasicConsume(queue: "q.header2",
                     autoAck: true,
                     consumer: consumer);
