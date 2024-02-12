// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;

Console.WriteLine("Producer");

var connectionFactory = new ConnectionFactory
{
    Uri = new Uri("amqps://lukszugl:fY5INvxK2sDQVPJlZvW23nXZfjDKK4lI@fish.rmq.cloudamqp.com/lukszugl")
};

using var connection = connectionFactory.CreateConnection();
using var channel = connection.CreateModel();

channel.ConfirmSelect();
channel.QueueDeclare("hello-queue2", durable: true, exclusive: false, autoDelete: false, arguments: null);

channel.BasicAcks += Channel_BasicAcks;

void Channel_BasicAcks(object? sender, RabbitMQ.Client.Events.BasicAckEventArgs e)
{
    Console.WriteLine($"Mesaj ack bilgisi geldi- {e.DeliveryTag}");
}

var message = "Merhaba RabbitMQ";

var body = Encoding.UTF8.GetBytes(message);

int count = 0;
Enumerable.Range(1, 100).ToList().ForEach(x =>
{
    channel.BasicPublish(exchange: "", routingKey: "hello-queue", basicProperties: null, body: body);


    count++;

    if (count % 30 == 0)
    {
        channel.WaitForConfirms();
    }


    Console.WriteLine($"Mesaj iletildi- {x}");
});


Console.WriteLine("Mesajlar gönderildi.");