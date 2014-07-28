using System;
using System.Data.Entity;
using Data.Context;
using Data.Context.Migrations;

namespace Data.Initialization
{
    /// <summary>
    /// Console application entry point, allowing manual migrating and population of the Stocks Tracker database.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<StocksTrackerContext, StocksTrackerMigrationsConfiguration>());

            using (var context = new StocksTrackerContext())
            {
                Console.WriteLine("Begin create/update of StocksTracker database...");
                try
                {
                    Console.WriteLine("Querying StocksTracker database...");
                    var stocks = context.Stocks;
                    foreach (var stock in stocks)
                    {
                        Console.WriteLine("Found stock {0}...", stock.TickerSymbol);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception creating/updating database: {0}", ex.Message);
                    Console.ReadKey();
                }

                Console.WriteLine("Finished.");
            }
        }
    }
}
