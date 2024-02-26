// See https://aka.ms/new-console-template for more information

using ConsoleApp1;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using var traceProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource("EmailSenderActivitySource")
    .ConfigureResource(configure =>
    {
        configure.AddService("Email.sender.app", serviceVersion: "1.0.0").AddAttributes(
            new List<KeyValuePair<string, object>>()
            {
                new("environment", "dev")
            });
    }).AddConsoleExporter().Build();


var httpService = new HttpService();
await httpService.MakeRequestToGoogle();


Console.WriteLine("Hello, World!");