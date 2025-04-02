using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RShopAPI_Test.Mapping;
using RShopAPI_Test.Services.Jwt;
using RShopAPI_Test.Services.Security;
using RShopAPI_Test.Services.UseCases.CreateCategory;
using RShopAPI_Test.Services.UseCases.CreateProduct;
using RShopAPI_Test.Services.UseCases.GetCatigoriesUseCase;
using RShopAPI_Test.Services.UseCases.GetProduct;
using RShopAPI_Test.Services.UseCases.GetProducts;
using RShopAPI_Test.Services.UseCases.LoginUseCase;
using RShopAPI_Test.Services.UseCases.Registration;
using RShopAPI_Test.Services.UseCases.UpdateProduct;
using RShopAPI_Test.Services.Validators;
using RShopAPI_Test.Storage;
using RShopAPI_Test.Storage.Interfaces;
using RShopAPI_Test.Storage.Mapping;
using RShopAPI_Test.Storage.Storages;

namespace RShopAPI_Test.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RShopDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Database"));
        });
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(RegistrationCommandValidator)));
        return services;
    }

    public static IServiceCollection AddAutoMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetAssembly(typeof(CategoryProfile)));
        services.AddAutoMapper(Assembly.GetAssembly(typeof(CreateProductRequestProfile)));
        return services;
    }

    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>();
        services.AddScoped<IGetCategoriesUseCase, GetCategoriesUseCase>();
        services.AddScoped<ICreateProductUseCase, CreateProductUseCase>();
        services.AddScoped<IGetProductUseCase, GetProductUseCase>();
        services.AddScoped<IGetAllProductsUseCase, GetAllProductsUseCase>();
        services.AddScoped<IUpdateProductUseCase, UpdateProductUseCase>();
        services.AddScoped<IGetProductsUseCase, GetProductsUseCase>();
        services.AddScoped<IRegistrationUseCase, RegistrationUseCase>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        return services;
    }

    public static IServiceCollection AddStorages(this IServiceCollection services)
    {
        services.AddScoped<ICreateCategoryStorage, CreateCategoryStorage>();
        services.AddScoped<IGetCategoriesStorage, GetCategoriesStorage>();
        services.AddScoped<ICreateProductStorage, CreateProductStorage>();
        services.AddScoped<IGetProductsStorage, GetProductsStorage>();
        services.AddScoped<IUpdateProductStorage, UpdateProductStorage>();
        services.AddScoped<IGetUserStorage, GetUserStorage>();
        services.AddScoped<ICreateUserStorage, CreateUserStorage>();
        return services;
    }

    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ISaltGenerator, SaltGenerator>();
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

        var jwtOptions = configuration.GetRequiredSection("JwtOptions").Get<JwtOptions>()!;
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(jwtOptions.SecretKey)),
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["my-cookies"];
                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }

    public static IServiceCollection AddJwtAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();
        return services;
    }
}