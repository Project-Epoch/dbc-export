using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace dbc_export
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory() + "/appsettings.json";

            Console.WriteLine(path);

            /** Check for config */
            if (! File.Exists(path)) {
                Console.WriteLine("Could not find 'appsettings.json'! Exiting.");

                Environment.Exit(-1);
            }

            /** Load Config */
            var config = new ConfigurationBuilder()
                .AddJsonFile(path, true, true)
                .Build();

            /** Init DB Connection */
            Database db = new Database(
                config["DB_HOST"],
                Int32.Parse(config["DB_PORT"]),
                config["DB_USER"],
                config["DB_PASS"],
                config["WORLD_DB"]
            );

            if (! db.Connect()) {
                Console.WriteLine("Failed to connect to DB! Exiting...");

                Environment.Exit(-1);
            }

            Console.WriteLine(String.Format("Connected to DB - MySQL {0}", db.GetConnection().ServerVersion));

            MySqlCommand command = new MySqlCommand("SELECT entry, name FROM creature_template", db.GetConnection());

            MySqlDataReader result = command.ExecuteReader();

            int rows = 0;
            while (result.Read())
            {
                Console.WriteLine(result[0]+" -- "+result[1]);

                rows++;
            }

            Console.WriteLine(String.Format("Found {0} Rows.", rows));

            result.Close();

            Console.WriteLine("Finished Query");

            db.Disconnect();
        }
    }
}
