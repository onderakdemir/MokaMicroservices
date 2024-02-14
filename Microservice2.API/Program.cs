using MassTransit;
using Microservice2.API.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedEventConsumer>();

    x.UsingRabbitMq((context, configure) =>
    {
        configure.Durable = true;

        configure.Host(
            new Uri("amqps://lukszugl:fY5INvxK2sDQVPJlZvW23nXZfjDKK4lI@fish.rmq.cloudamqp.com/lukszugl"));


        configure.ReceiveEndpoint("microservice2.order.created.event",
            configureEndpoint => { configureEndpoint.ConfigureConsumer<OrderCreatedEventConsumer>(context); });
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();