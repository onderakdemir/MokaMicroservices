using MassTransit;
using SharedEvents;

namespace Microservice2.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        public Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            Console.WriteLine($"{context.Message.OrderCode}- {context.Message.TotalPrice}");

            return Task.CompletedTask;
        }
    }
}