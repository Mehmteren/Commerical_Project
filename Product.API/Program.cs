using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Product.API.Data;
using Product.API.Data.Entities.ViewModels;
using Product.API.Data.Repository.CategoryProductRepository;
using Product.API.Data.Repository.CategoryRepository;
using Product.API.Data.Repository.ProductRepository;
using Product.API.DTOS.CategoryDTO.Validator;
using Product.API.DTOS.ProductCategoryDTO.Validator;
using Product.API.DTOS.ProductDTO.Validator;
using Product.API.service.CategoryService;
using Product.API.service.ProductCategoryService;
using Product.API.service.ProductService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// DbContext configuration
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(Program));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();





// Repository and Service registrations
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Validator'lar� kaydedin
builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryDtoValidator>();


// Repository and Service registrations
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();

// Validator'lar� kaydedin
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCategoryDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductCategoryDtoValidator>();



// Repository and Service registrations
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// Validator'lar� kaydedin
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductDtoValidator>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapPost("/create-all", async (ProductDbContext context, CreateAllRequest request) =>
{
    // Verilerin do�rulu�unu kontrol et
    if (request.Product == null)
        return Results.BadRequest("�r�n verisi gerekli.");

    if (request.Category == null)
        return Results.BadRequest("Kategori verisi gerekli.");

    // Transaction ba�lat
    using var transaction = await context.Database.BeginTransactionAsync();

    try
    {
        // 1. Kategoriyi kontrol et, ayn� isimli kategori varsa onu kullan
        var existingCategory = await context.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == request.Category.Name.ToLower());

        int categoryId;

        if (existingCategory != null)
        {
            // Var olan kategoriyi kullan
            categoryId = existingCategory.Id;
        }
        else
        {
            // Yeni kategori olu�tur
            var category = new Product.API.Data.Entities.Category
            {
                Name = request.Category.Name,
                Description = request.Category.Description,
                Active = request.Category.Active
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            categoryId = category.Id;
        }

        // 2. �r�n� olu�tur
        var product = new Product.API.Data.Entities.Product
        {
            Name = request.Product.Name,
            Price = request.Product.Price
        };

        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        // 3. �r�n-Kategori ili�kisini olu�tur
        var productCategory = new Product.API.Data.Entities.ProductCategory
        {
            ProductId = product.Id,
            CategoryId = categoryId
        };

        await context.ProductCategories.AddAsync(productCategory);
        await context.SaveChangesAsync();

        // ��lemleri kaydet
        await transaction.CommitAsync();

        // Sonucu d�nd�r
        var categoryResult = await context.Categories.FindAsync(categoryId);

        return Results.Created("/create-all", new
        {
            Product = new { Id = product.Id, Name = product.Name, Price = product.Price },
            Category = new
            {
                Id = categoryResult.Id,
                Name = categoryResult.Name,
                Description = categoryResult.Description,
                Active = categoryResult.Active
            },
            Relationship = new
            {
                Id = productCategory.Id,
                ProductId = productCategory.ProductId,
                CategoryId = productCategory.CategoryId
            }
        });
    }
    catch (Exception ex)
    {
        // Hata durumunda i�lemleri geri al
        await transaction.RollbackAsync();
        return Results.Problem($"��lem s�ras�nda hata olu�tu: {ex.Message}", statusCode: 500);
    }
});

// �stek modeli






app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
