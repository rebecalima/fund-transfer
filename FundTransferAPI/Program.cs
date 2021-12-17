using Serilog;
using Serilog.Sinks.Elasticsearch;
using FundTransferAPI.Interface;
using FundTransferAPI.Services;
using FundTransferAPI.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .WriteTo.Elasticsearch(
        new ElasticsearchSinkOptions(
            new Uri(builder.Configuration["ElasticConfiguration:Uri"]))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"transferencia-bancaria-logs-{DateTime.UtcNow:yyyy-MM}"
        }));

builder.Services.AddHostedService<ConsumerService>();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddSingleton<IElasticSearchService, ElasticSearchService>();
builder.Services.AddSingleton<ITransferService, TransferService>();
builder.Services.AddSingleton<IProducerService, ProducerService>();
builder.Services.AddTransient<IClientAPI, AcessoClientAPI>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
