using Microsoft.OpenApi.Models;
using POC_Redis.Repositories;
using POC_Redis.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var endpoint = builder.Configuration.GetValue<string>("Redis:Endpoint");

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.InstanceName = "RedisInstance";
    options.Configuration = endpoint;   
});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PoC_Redis"
    });
});

builder.Services.AddScoped<ICacheRepository, RedisRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PoC_Redis v1");
});

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
