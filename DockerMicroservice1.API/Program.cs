using System.Data.Common;
using System.Security.Claims;
using DockerMicroservice1.API.Models;
using DockerMicroservice1.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
builder.Services.AddHttpClient<Microservice2Services>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration.GetSection("MicroservicesUrl")["Microservice2BaseUrl"]!);
});

builder.Services.AddOpenTelemetry().WithTracing(configure =>
{
    configure.SetSampler(new TraceIdRatioBasedSampler(1));
    configure.AddSource("DockerMicroservice1ActivitySource");

    configure.ConfigureResource(resourceConfigure =>
    {
        resourceConfigure.AddService("DockerMicroservice1", serviceVersion: "1.00");
    });


    configure.AddAspNetCoreInstrumentation(aspnetCoreConfigure =>
    {
        aspnetCoreConfigure.Filter = (httpContext) =>
        {
            if (!httpContext.Request.Path.HasValue || !httpContext.Request.Path.Value.Contains("api")) return false;


            return true;
        };

        aspnetCoreConfigure.EnrichWithHttpRequest = ((activity, request) =>
        {
            var claim = request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            var userId = 134;

            activity.SetTag("userId", userId);
        });
    });

    configure.AddEntityFrameworkCoreInstrumentation(entityConfigure =>
    {
        entityConfigure.EnrichWithIDbCommand = (activity, dbCommand) =>
        {
            foreach (DbParameter parameter in dbCommand.Parameters)
            {
                activity.SetTag(parameter.ParameterName, parameter.Value);
            }
        };


        entityConfigure.SetDbStatementForStoredProcedure = true;
        entityConfigure.SetDbStatementForText = true;
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
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//    dbContext.Database.Migrate();
//}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//}
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();

app.Run();