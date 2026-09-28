// If no arguments are specified run WebApi, if there are arguments run console app
using BSDExchange.Api;
using BSDExchange.ConsoleApplication;

if (args.Length > 0)
    return ConsoleApp.Run(args);

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