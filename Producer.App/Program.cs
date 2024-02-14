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

//channel.BasicAcks += Channel_BasicAcks;

channel.BasicReturn += Channel_BasicReturn;

void Channel_BasicReturn(object? sender, RabbitMQ.Client.Events.BasicReturnEventArgs e)
{
    Console.WriteLine($"Mesaj ilgili kuyruğa gitmedi.{e.RoutingKey}");
}

//void Channel_BasicAcks(object? sender, RabbitMQ.Client.Events.BasicAckEventArgs e)
//{
//    Console.WriteLine($"Mesaj ack bilgisi geldi- {e.DeliveryTag}");
//}


var message = "Merhaba RabbitMQ";

var body = Encoding.UTF8.GetBytes(message);


channel.ExchangeDeclare("direct-exchange", ExchangeType.Direct, true, false, null);

Enumerable.Range(1, 10).ToList().ForEach(x =>
{
    var properties = channel.CreateBasicProperties();
    properties.Persistent = true;
    channel.BasicPublish(exchange: "direct-exchange", routingKey: "order.created.route.key",
        basicProperties: properties,
        body: body, mandatory: true);


    Console.WriteLine($"Mesaj iletildi- {x}");
});


Console.WriteLine("Mesajlar gönderildi.");