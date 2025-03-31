using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RShopAPI_Test.Middlewares;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Services.UseCases;
using RShopAPI_Test.Services.UseCases.CreateCategory;
using RShopAPI_Test.Services.UseCases.GetCatigoriesUseCase;
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

builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(CategoryProfile)));

builder.Services.AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>();
builder.Services.AddScoped<ICreateCategoryStorage, CreateCategoryStorage>();

builder.Services.AddScoped<IGetCategoriesUseCase, GetCategoriesUseCase>();
builder.Services.AddScoped<IGetCategoriesStorage, GetCategoriesStorage>();

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