using BSDExchange.Configuration;
using BSDExchange.Helpers;

namespace BSDExchange;

public static class WebApi
{
    public static void Run(string[] args, DataFilesOptions dataFiles)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();

        var data = ExchangeDataLoader.Load(dataFiles.OrderBooksPath, dataFiles.BalancesPath);
        builder.Services.AddSingleton(data);

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();


        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

