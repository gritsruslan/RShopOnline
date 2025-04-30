using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Minio;
using RShopAPI_Test.Mapping;
using RShopAPI_Test.Services.Authentication;
using RShopAPI_Test.Services.Authorization;
using RShopAPI_Test.Services.Authorization.Resolvers;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Services.Jwt;
using RShopAPI_Test.Services.Security;
using RShopAPI_Test.Services.Services;
using RShopAPI_Test.Services.Validators;
using RShopAPI_Test.Services.Validators.Services;
using RShopAPI_Test.Storage;
using RShopAPI_Test.Storage.Interfaces;
using RShopAPI_Test.Storage.Mapping;
using RShopAPI_Test.Storage.Minio;
using RShopAPI_Test.Storage.Repositories;
using Serilog;
using Serilog.Filters;
using IValidatorFactory = RShopAPI_Test.Services.Validators.Services.IValidatorFactory;

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

    public static IServiceCollection AddAppLogging(
        this IServiceCollection services, 
        IConfiguration configuration, 
        string environmentName)
    {
        services.AddLogging(b => b.AddSerilog(
            new LoggerConfiguration()
                .MinimumLevel.Warning()
                .Enrich.WithProperty("Application", "RShopOnline.API")
                .Enrich.WithProperty("Environment", environmentName)
                .WriteTo.Logger(
                    lc => lc.Filter.ByExcluding(
                        Matching.FromSource("Microsoft")).WriteTo.OpenSearch(
                        configuration.GetConnectionString("Logs"),
                        "rshop-logs-{0:dd.MM.yyyy}"))
                .WriteTo.Logger(lc => lc.WriteTo.Console())
                .CreateLogger()));

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(RegistrationCommandValidator)));
        services.AddScoped<IValidatorFactory, ValidatorFactory>();
        return services;
    }

    public static IServiceCollection AddAutoMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetAssembly(typeof(CategoryProfile)))
                .AddAutoMapper(Assembly.GetAssembly(typeof(CreateProductRequestProfile)));
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>()
            .AddScoped<ICategoryService, CategoryService>()
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IOrderService, OrderService>()
            .AddScoped<IImageService, ImageService>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICategoriesRepository, CategoriesRepository>()
            .AddScoped<IUsersRepository, UsersRepository>()
            .AddScoped<IProductsRepository, ProductsRepository>()
            .AddScoped<IOrdersRepository, OrdersRepository>()
            .AddScoped<IImagesRepository, ImagesRepository>();
        return services;
    }

    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>()
                .AddScoped<ISaltGenerator, SaltGenerator>();
        return services;
    }

    public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
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

        services.AddScoped<IIdentityProvider, IdentityProvider>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }

    public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
    {
        services.AddScoped<IIntentionManager, IntentionManager>();
        services.AddScoped<IIntentionResolver, CancelOrderIntentionResolver>();
        services.AddScoped<IIntentionResolver, ChangePasswordIntentionResolver>();
        services.AddScoped<IIntentionResolver, CreateCategoryIntentionResolver>();
        services.AddScoped<IIntentionResolver, CreateOrderIntentionResolver>();
        services.AddScoped<IIntentionResolver, CreateProductIntentionResolver>();
        services.AddScoped<IIntentionResolver, GetOrderByIdIntentionResolver>();
        services.AddScoped<IIntentionResolver, GetOrdersByCurrentUserIntentionResolver>();
        services.AddScoped<IIntentionResolver, UpdateOrderStatusIntentionResolver>();
        services.AddScoped<IIntentionResolver, UpdateProductIntentionResolver>();
        return services;
    }

    public static IServiceCollection AddMinio(this IServiceCollection services)
    {
        services.AddSingleton<IMinioClient, MinioClient>(_ =>
        {
            var minioClient = new MinioClient()
                .WithEndpoint("localhost:9000")
                .WithCredentials("admin", "admin123")
                .WithSSL(false)
                .Build();
            return (MinioClient) minioClient;
        });
        services.AddScoped<IImagesMinioStorage, ImagesMinioStorage>();
        return services;
    }
}