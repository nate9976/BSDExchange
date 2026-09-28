using BSDExchange.Configuration;
using BSDExchange.Helpers;
using BSDExchange.Models.OB;

namespace BSDExchange.ConsoleApplication
{
    public class ConsoleApp
    {
        public static int Run(string[] args, DataFilesOptions dataFiles)
        {
            var data = OrderBookParser.ParseFile(dataFiles.OrderBooksPath);

            return 0;
        }
    }
}
