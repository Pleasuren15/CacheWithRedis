using CacheWithRedis.Api.Services;
using System.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IDbConnection>(provider =>
    new SqlConnection(builder.Configuration.GetConnectionString("PsDevs")));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "CacheWithRedis_";
});

builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<ISubscriberService, SubscriberService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Redirect root to Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.Run();
