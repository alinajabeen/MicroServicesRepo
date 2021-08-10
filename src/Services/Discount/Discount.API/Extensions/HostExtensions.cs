using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailabilty = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                try
                {
                    logger.LogInformation("Migrating Postgresql database.");
                    //First we will build the Connection
                    using var connection = new NpgsqlConnection
                        (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();
                    //Then we initialize the Command using connection
                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };
                    //Sql Commands
                    command.CommandText = "Drop table if Exists Coupon";
                    //
                    command.ExecuteNonQuery();
                    command.CommandText = @"Create Table Coupon(Id Serial Primary Key,
                                                                   ProductName varchar(24) not null,
                                                                   Description Text,
                                                                   Amount int)";
                    command.ExecuteNonQuery();
                    command.CommandText = "Insert into Coupon(ProductName,Description,Amount) Values('IPhone X','3 rare Cameras',120)";
                    command.ExecuteNonQuery();
                    command.CommandText = "Insert into Coupon(ProductName,Description,Amount) Values('Samsung S7','Front Camera with sensors',1500)";
                    command.ExecuteNonQuery();
                    logger.LogInformation("Migrated postgresql database.");
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex,"An Error occured while migrating the postgresql database");
                    if (retryForAvailabilty < 50)
                    {
                        retryForAvailabilty++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host,retryForAvailabilty);
                    }
                }
            }
            return host;
        }
           
    }

}
