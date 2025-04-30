using RShopAPI_Test.Extensions;
using RShopAPI_Test.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddAppLogging(configuration, builder.Environment.EnvironmentName);
    
builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();

builder.Services
    .AddDatabase(configuration)
    .AddValidators()
    .AddAutoMapping()
    .AddServices()
    .AddRepositories()
    .AddSecurity()
    .AddMinio()
    .AddApiAuthentication(configuration)
    .AddApiAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AssertMapperConfigurationIsValid();

app.UseMiddleware<GlobalExceptionHandler>();
app.UseHttpsRedirection();
app.MapControllers();

app.UseAuthentication();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseAuthorization();

app.Run();

public partial class Program {}