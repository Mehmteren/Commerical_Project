using Category.API.Data;
using Category.API.Data.Repository;
using Category.API.DTOS.Validators;
using Category.API.services;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// -- Veritaban� ba�lant�s�
builder.Services.AddDbContext<CategoryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// -- AutoMapper, Repository, Service
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// -- FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryDtoValidator>();

// -- Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -- MassTransit (RabbitMQ)
builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);
    });
});

// 1?? Product API'ye REST istekleri atmak i�in HttpClient kayd�
// appsettings.json veya environment variable i�inden "ProductApiBaseUrl" �ekiyoruz.
// E�er de�er bo� gelirse, localhost:6001 gibi bir varsay�lan de�er atayabiliriz.
var productApiBaseUrl = builder.Configuration["ProductApiBaseUrl"] 
                       ?? "https://localhost:7234"; // Fallback �rne�i

builder.Services.AddHttpClient("ProductApi", client =>
{
    client.BaseAddress = new Uri(productApiBaseUrl);
});

var app = builder.Build();

// -- Dev ortam� i�in Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
