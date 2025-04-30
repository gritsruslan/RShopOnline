using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Minio;
using RShopAPI_Test.Mapping;
using RShopAPI_Test.Services.Authentication;
using RShopAPI_Test.Services.Authentication.Jwt;
using RShopAPI_Test.Services.Authorization;
using RShopAPI_Test.Services.Authorization.Resolvers;
using RShopAPI_Test.Services.Interfaces;
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
        IWebHostEnvironment environment)
    {
        services.AddLogging(b => b.AddSerilog(
            new LoggerConfiguration()
                .MinimumLevel.Warning()
                .Enrich.WithProperty("Application", "RShopOnline.API")
                .Enrich.WithProperty("Environment", environment.EnvironmentName)
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
        return services
            .AddValidatorsFromAssembly(typeof(RegistrationCommandValidator).Assembly)
            .AddScoped<IValidatorFactory, ValidatorFactory>();
    }

    public static IServiceCollection AddAutoMapping(this IServiceCollection services)
    {
        return services
            .AddAutoMapper(typeof(CategoryProfile).Assembly)
            .AddAutoMapper(typeof(CreateProductRequestProfile).Assembly);
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<ICategoryService, CategoryService>()
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IOrderService, OrderService>()
            .AddScoped<IImageService, ImageService>();
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<ICategoriesRepository, CategoriesRepository>()
            .AddScoped<IUsersRepository, UsersRepository>()
            .AddScoped<IProductsRepository, ProductsRepository>()
            .AddScoped<IOrdersRepository, OrdersRepository>()
            .AddScoped<IImagesRepository, ImagesRepository>();
    }

    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        return services.AddScoped<IPasswordHasher, PasswordHasher>()
                .AddScoped<ISaltGenerator, SaltGenerator>();
    }

    public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

        var jwtOptions = configuration.GetRequiredSection(nameof(JwtOptions)).Get<JwtOptions>()!;
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
                        context.Token = context.Request.Cookies[CookieNames.AuthToken];
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
        return services.AddScoped<IIntentionManager, IntentionManager>()
            .AddScoped<IIntentionResolver, CancelOrderIntentionResolver>()
            .AddScoped<IIntentionResolver, ChangePasswordIntentionResolver>()
            .AddScoped<IIntentionResolver, CreateCategoryIntentionResolver>()
            .AddScoped<IIntentionResolver, CreateOrderIntentionResolver>()
            .AddScoped<IIntentionResolver, CreateProductIntentionResolver>()
            .AddScoped<IIntentionResolver, GetOrderByIdIntentionResolver>()
            .AddScoped<IIntentionResolver, GetOrdersByCurrentUserIntentionResolver>()
            .AddScoped<IIntentionResolver, UpdateOrderStatusIntentionResolver>()
            .AddScoped<IIntentionResolver, UpdateProductIntentionResolver>();
    }

    public static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(nameof(MinioOptions)));
        
        var minioOptions = configuration.GetRequiredSection(nameof(MinioOptions)).Get<MinioOptions>()!;
        services.AddSingleton<IMinioClient, MinioClient>(_ =>
        {
            var minioClient = new MinioClient()
                .WithEndpoint(minioOptions.Endpoint)
                .WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey)
                .WithSSL(false)
                .Build();
            return  (MinioClient) minioClient;
        });
        services.AddScoped<IImagesMinioStorage, ImagesMinioStorage>();
        return services;
    }
}