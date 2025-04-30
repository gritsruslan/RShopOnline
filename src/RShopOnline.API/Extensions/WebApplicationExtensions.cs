using AutoMapper;

namespace RShopAPI_Test.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication AssertMapperConfigurationIsValid(this WebApplication app)
    {
        app.Services.GetRequiredService<IMapper>().ConfigurationProvider.AssertConfigurationIsValid();
        return app;
    }
}