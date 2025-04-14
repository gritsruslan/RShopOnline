using RShopAPI_Test.Extensions;
using RShopAPI_Test.Middlewares;
using Serilog;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddLogging(b => b.AddSerilog(
    new LoggerConfiguration()
        .MinimumLevel.Warning()
        .Enrich.WithProperty("Application", "RShopOnline.API")
        .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
        .WriteTo.Logger(
            lc => lc.Filter.ByExcluding(
                Matching.FromSource("Microsoft")).WriteTo.OpenSearch(
                configuration.GetConnectionString("Logs"),
                        "rshop-logs-{0:dd.MM.yyyy}"))
        .WriteTo.Logger(lc => lc.WriteTo.Console())
        .CreateLogger()));

    
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

app.UseMiddleware<GlobalExceptionHandler>();
app.UseHttpsRedirection();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();