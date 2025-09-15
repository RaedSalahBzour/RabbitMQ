using RabbitMQ.Stream.Client;
using RabbitMQ.Stream.Client.Reliable;
using System.Buffers;
using System.Net;
using System.Text;

await ConsumeStreamMessages();

async Task ConsumeStreamMessages()
{
    var config = new StreamSystemConfig
    {
        UserName = "raed",
        Password = "raed",
        VirtualHost = "/",
        Endpoints = new List<EndPoint>
        {
            new IPEndPoint(IPAddress.Loopback, 5552), // node 1
            new IPEndPoint(IPAddress.Loopback, 5551)  // node 2
        }
    };

    var system = await StreamSystem.Create(config);
    const string stream = "stQueue-1";

    var consumerConfig = new ConsumerConfig(system, stream)
    {
        Reference = "stQueueRef",

        // You can choose where to start consuming
        // OffsetSpec = new OffsetTypeFirst(),    // start from the first message
        // OffsetSpec = new OffsetTypeLast(),     // start from the last message
        // OffsetSpec = new OffsetTypeOffset(5),  // start from message 5
        OffsetSpec = new OffsetTypeNext(),        // start from the next message

        // Receiving messages
        MessageHandler = async (sourceStream, consumer, ctx, message) =>
        {
            Console.WriteLine($" message : Stream name : {sourceStream}, data : {Encoding.Default.GetString(message.Data.Contents.ToArray())}");
            Console.WriteLine();
            await Task.CompletedTask;
        }
    };
    var consumer = await Consumer.Create(consumerConfig);

    Console.WriteLine(" -------------------- ");
    Console.ReadLine();
    await consumer.Close();
    await system.Close();
}