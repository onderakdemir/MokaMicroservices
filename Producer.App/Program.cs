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


channel.QueueDeclare("hello-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);


var message = "Merhaba RabbitMQ";

var body = Encoding.UTF8.GetBytes(message);

Enumerable.Range(1, 1000).ToList().ForEach(x =>
{
    channel.BasicPublish(exchange: "", routingKey: "hello-queue", basicProperties: null, body: body);
});


Console.WriteLine("Mesajlar gönderildi.");