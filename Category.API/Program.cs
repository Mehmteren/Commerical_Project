using Category.API.Data;
using Category.API.Data.Repository;
using Category.API.DTOS.Validators;
using Category.API.services;
using Category.API.Services;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;

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

// -- Product API HttpClient yap�land�rmas�
// appsettings.json i�inden ProductApiBaseUrl al�n�yor, yoksa do�rudan container ad� kullan�l�yor
// DNS ��z�mleme i�in do�ru container ad� kullan�lmal�
builder.Services.AddHttpClient("Product.API", client =>
{
    // Container ad�n� do�ru kullan�n ve timeout ekleyin
    client.BaseAddress = new Uri("http://Product.API:8080");
    client.Timeout = TimeSpan.FromSeconds(30); // Timeout de�eri
});

var app = builder.Build();

// -- Development ortam� i�in Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();