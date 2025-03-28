using Microsoft.EntityFrameworkCore;
using Basket.API.Data;
using Basket.API.Mapping;
using FluentValidation;
using Basket.API.DTOS.BasketDTO.Validators;
using Basket.API.Data.Repository.Basket;
using Basket.API.Services.BasketService;
using Basket.API.Data.Repository;
using Basket.API.DTOS.Validators;
using Basket.API.Services.BasketServices;
using Basket.API.Services;
using MassTransit;
using Shared.Events.BasketEvents;
using Shared.Settings;
using Basket.API.Data.ViewModels;
using Basket.API.Data.Entities;
using System.Linq;
using Basket.API.Consumers;

// Web uygulamas� olu�turma
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<BasketDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "BasketCache:"; 
});

// MassTransit (mesaj kuyruk sistemi) 
builder.Services.AddMassTransit(configurator =>
{
    configurator.AddRequestClient<ProductAddedToBasketRequestEvent>();

    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);
    });
});

//Dependency Injection kayd�
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IBasketItemRepository, BasketItemRepository>();
builder.Services.AddScoped<IBasketItemService, BasketItemService>();

builder.Services.AddAutoMapper(typeof(BasketAutoMapperProfile));

builder.Services.AddValidatorsFromAssemblyContaining<CreateBasketValidators>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateBasketValidators>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBasketItemValidators>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateBasketItemValidators>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Minimal API endpoint: Sepete �r�n ekleme
app.MapPost("/add-to-basket", async (
    AddToBasketVM model,                      // �stek modelimiz
    IBasketRepository basketRepository,      
    IBasketItemRepository basketItemRepository, 
    BasketDbContext context,                  
    ISendEndpointProvider sendEndpointProvider) => 
{

    Baskett baskett = new()
    {
        UserId = model.UserId,
        TotalPrice = model.BasketItems.Sum(bi => bi.Price * bi.Count),
        // Sepet ��elerini model �zerinden d�n��t�rme
        BasketItems = model.BasketItems.Select(bi => new BasketItem
        {
            Price = bi.Price,
            Count = bi.Count,
            ProductId = bi.ProductId,
        }).ToList(),
    };

    await context.Baskets.AddAsync(baskett);
    await context.SaveChangesAsync();

    // Saga State Machine'e g�nderilecek event olu�turma
    ProductAddedToBasketRequestEvent productAddedEvent = new()
    {
        ProductId = model.ProductId,
        Count = model.Count,
        UserId = baskett.UserId,
        Price = model.Price,
        // Sepetteki t�m �r�nleri mesajlara d�n��t�rme
        BasketItemMessages = baskett.BasketItems.Select(bi => new Shared.Messages.BasketItemMessage
        {
            Price = bi.Price,
            Count = bi.Count,
            ProductId = bi.ProductId,
        }).ToList(),
    };

    // RabbitMQ kuyru�una mesaj g�nderme i�in endpoint olu�turma
    var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(
        new Uri($"queue:{RabbitMQSettings.StateMachineQueue}"));

    // Event'i kuyru�a g�nderme
    await sendEndpoint.Send<ProductAddedToBasketRequestEvent>(productAddedEvent);
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();
app.Run();