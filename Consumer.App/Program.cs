// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Consumer");


var connectionFactory = new ConnectionFactory
{
    Uri = new Uri("amqps://lukszugl:fY5INvxK2sDQVPJlZvW23nXZfjDKK4lI@fish.rmq.cloudamqp.com/lukszugl")
};

using var connection = connectionFactory.CreateConnection();
using var channel = connection.CreateModel();

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (sender, e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine($"Gelen mesaj: {message}");
    //channel.BasicAck(e.DeliveryTag, multiple: false);
};

channel.BasicConsume("hello-queue", true, consumer);

Console.ReadLine();