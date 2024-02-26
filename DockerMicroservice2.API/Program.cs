using System.Security.Claims;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddOpenTelemetry().WithTracing(configure =>
{
    configure.AddSource("DockerMicroservice2ActivitySource");

    configure.ConfigureResource(resourceConfigure =>
    {
        resourceConfigure.AddService("DockerMicroservice2", serviceVersion: "1.00");
    });


    configure.AddAspNetCoreInstrumentation(aspnetCoreConfigure =>
    {
        aspnetCoreConfigure.Filter = (httpContext) =>
        {
            if (!httpContext.Request.Path.HasValue || !httpContext.Request.Path.Value.Contains("api")) return false;


            return true;
        };

        aspnetCoreConfigure.EnrichWithHttpRequest = ((activity, request) => { });
    });


    configure.AddHttpClientInstrumentation(httpConfigure =>
    {
        httpConfigure.EnrichWithHttpRequestMessage = (async (activity, message) =>
        {
            if (message.Content == null) return;


            var requestBodyContent = await message.Content.ReadAsStringAsync();

            activity.SetTag("request.body", requestBodyContent);
        });

        httpConfigure.EnrichWithHttpResponseMessage = (async (activity, message) =>
        {
            activity.SetTag("response.body", await message.Content.ReadAsStringAsync());
        });
    });


    configure.AddConsoleExporter().AddOtlpExporter();
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();