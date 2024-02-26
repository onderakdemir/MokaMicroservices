// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
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
    }).AddConsoleExporter().AddOtlpExporter().Build();


using var traceProvider2 = Sdk.CreateTracerProviderBuilder()
    .AddSource("EmailSenderActivitySourceToWriteFile")
    .ConfigureResource(configure =>
    {
        configure.AddService("Email.sender.app", serviceVersion: "1.0.0").AddAttributes(
            new List<KeyValuePair<string, object>>()
            {
                new("environment", "dev")
            });
    }).Build();

ActivitySource.AddActivityListener(new ActivityListener()
{
    ShouldListenTo = source => source.Name == "EmailSenderActivitySourceToWriteFile",
    ActivityStarted = activity => { Console.WriteLine($"Activity started: {activity.OperationName}"); },
    ActivityStopped = activity => { Console.WriteLine($"Activity stopped: {activity.OperationName}"); }
});


var httpService = new HttpService();

//Console.WriteLine(await httpService.MakeRequestToAmazon());
await httpService.MakeRequestToGoogle();


Console.WriteLine("Hello, World!");