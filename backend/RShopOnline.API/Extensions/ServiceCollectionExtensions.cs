using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RShopAPI_Test.Mapping;
using RShopAPI_Test.Services.Auth;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Services.Jwt;
using RShopAPI_Test.Services.Security;
using RShopAPI_Test.Services.Services;
using RShopAPI_Test.Services.Validators;
using RShopAPI_Test.Storage;
using RShopAPI_Test.Storage.Interfaces;
using RShopAPI_Test.Storage.Mapping;
using RShopAPI_Test.Storage.Repositories;

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

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IProductsRepository, ProductsRepository>();
        services.AddScoped<IOrdersRepository, OrdersRepository>();
        return services;
    }

    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ISaltGenerator, SaltGenerator>();
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

        return services;
    }

    public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, RolePolicyProvider>();
        return services;
    }
}