using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RShopAPI_Test.Mapping;
using RShopAPI_Test.Middlewares;
using RShopAPI_Test.Services.Security;
using RShopAPI_Test.Services.UseCases.CreateCategory;
using RShopAPI_Test.Services.UseCases.CreateProduct;
using RShopAPI_Test.Services.UseCases.GetCatigoriesUseCase;
using RShopAPI_Test.Services.UseCases.GetProduct;
using RShopAPI_Test.Services.UseCases.GetProducts;
using RShopAPI_Test.Services.UseCases.Registration;
using RShopAPI_Test.Services.UseCases.UpdateProduct;
using RShopAPI_Test.Services.Validators;
using RShopAPI_Test.Storage;
using RShopAPI_Test.Storage.Interfaces;
using RShopAPI_Test.Storage.Mapping;
using RShopAPI_Test.Storage.Storages;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<RShopDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("Database"));
});
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(RegistrationCommandValidator)));

builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(CategoryProfile)));
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(CreateProductRequestProfile)));

builder.Services.AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>();
builder.Services.AddScoped<ICreateCategoryStorage, CreateCategoryStorage>();

builder.Services.AddScoped<IGetCategoriesUseCase, GetCategoriesUseCase>();
builder.Services.AddScoped<IGetCategoriesStorage, GetCategoriesStorage>();

builder.Services.AddScoped<ICreateProductUseCase, CreateProductUseCase>();
builder.Services.AddScoped<ICreateProductStorage, CreateProductStorage>();

builder.Services.AddScoped<IGetProductUseCase, GetProductUseCase>();
builder.Services.AddScoped<IGetAllProductsUseCase, GetAllProductsUseCase>();
builder.Services.AddScoped<IGetProductsStorage, GetProductsStorage>();

builder.Services.AddScoped<IUpdateProductUseCase, UpdateProductUseCase>();
builder.Services.AddScoped<IUpdateProductStorage, UpdateProductStorage>();

builder.Services.AddScoped<IGetProductsUseCase, GetProductsUseCase>();

builder.Services.AddScoped<IRegistrationUseCase, RegistrationUseCase>();
builder.Services.AddScoped<IGetUserStorage, GetUserStorage>();
builder.Services.AddScoped<ICreateUserStorage, CreateUserStorage>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ISaltGenerator, SaltGenerator>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();