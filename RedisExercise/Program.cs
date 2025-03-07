using Microsoft.EntityFrameworkCore;
using RedisExercise.Context;
using RedisExercise.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var multiplexer = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"));

builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = builder.Configuration.GetConnectionString("Redis");
	options.InstanceName = "RedisInstance";
});

builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

builder.Services.AddDbContext<DbRedis>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<ICacheService, RedisCacheService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
