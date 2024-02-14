using MassTransit;
using SharedEvents;

namespace Microservice2.API.Consumers
{
    public class UserCraetedEventConsumer : IConsumer<UserCreatedEvent>
    {
        public Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            Console.WriteLine(context.Message.Email);
            return Task.CompletedTask;
        }
    }
}