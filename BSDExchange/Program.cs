using BSDExchange.Api;
using BSDExchange.Configuration;
using BSDExchange.ConsoleApplication;

// Load configuration to get file locations
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var dataFiles = configuration.GetSection("DataFiles").Get<DataFilesOptions>();
if (string.IsNullOrWhiteSpace(dataFiles?.OrderBooksPath))
{
    Console.WriteLine("Error: appsettings.json must set DataFiles:OrderBooksPath");
    return 1;
}

if (args.Length > 0)
    return ConsoleApp.Run(args);

// If no arguments are specified run WebApi, if there are arguments run console app
try
{
    WebApi.Run(args);
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    return 1;
}
return 0;