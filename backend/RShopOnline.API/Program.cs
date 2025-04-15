using AutoMapper;
using RShopAPI_Test.Extensions;
using RShopAPI_Test.Middlewares;
using Serilog;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddAppLogging(configuration, builder.Environment.EnvironmentName);
    
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();
builder.Services.AddDatabase(configuration);
builder.Services.AddValidators();
builder.Services.AddAutoMapping();
builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddSecurity();
builder.Services.AddApiAuthentication(configuration);
builder.Services.AddApiAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Services.GetRequiredService<IMapper>().ConfigurationProvider.AssertConfigurationIsValid();

app.UseMiddleware<GlobalExceptionHandler>();
app.UseHttpsRedirection();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

public partial class Program {}