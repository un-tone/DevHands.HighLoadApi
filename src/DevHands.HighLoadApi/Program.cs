using DevHands.HighLoadApi.Data;
using StackExchange.Redis;

namespace DevHands.HighLoadApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddTransient<IDapperContext, DapperContext>();
        builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
            ConnectionMultiplexer.Connect(provider.GetRequiredService<IConfiguration>()
                .GetConnectionString("RedisCache") ?? string.Empty));
        builder.Services.AddKeyedScoped<IDataStorage, CacheStorage>("cache");
        builder.Services.AddKeyedScoped<IDataStorage, DbStorage>("db");

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}