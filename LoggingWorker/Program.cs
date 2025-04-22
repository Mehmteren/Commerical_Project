using LoggingWorker.Consumers;
using Logging.Shared.Models;
using MassTransit;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using LoggingWorker;

var logger = new LoggerConfiguration()
    .WriteTo.Console()
.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200"))
{
    AutoRegisterTemplate = true,
    IndexFormat = "central-logssss-{0:yyyy-MM}", // $ i�aretini kald�r�n
    TypeName = null // ES 8.x i�in gerekli
})
    .CreateLogger();

// ? Eski modeldeki gibi builder yerine do�rudan Host ile ba�lan�r
var host = Host.CreateDefaultBuilder(args)
    .UseSerilog(logger) // Art�k do�ru yerde
    .ConfigureServices((context, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<LoggingEventConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(new Uri(context.Configuration["RabbitMQ"]), _ => { });

                cfg.ReceiveEndpoint("log-queue", e =>
                {
                    e.ConfigureConsumer<LoggingEventConsumer>(ctx);
                });
            });
        });

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();