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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<BasketDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "BasketCache:";
});


builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);

        // Basket_ProductAddedToBasketQueue kuyru�una g�nderim yap�land�rmas�
        _configure.Send<ProductAddedToBasketRequestEvent>(x =>
        {
            x.UseRoutingKeyFormatter(context => "Basket_ProductAddedToBasketQueue");
        });
    });
});

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
