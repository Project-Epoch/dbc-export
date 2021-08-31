using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using CLIHelpers;
namespace dbc_export
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory() + "/appsettings.json";

            /** Check for config */
            if (!File.Exists(path))
            {
                Logger.Danger("Could not find 'appsettings.json'! Exiting.");

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

            if (!db.Connect())
            {
                Logger.Danger("Failed to connect to DB! Exiting...");

                Environment.Exit(-1);
            }

            Logger.Success(String.Format("Connected to DB - MySQL {0}\n", db.GetConnection().ServerVersion));

            ParsedDefinitions parsedDefinitions;

            /** Deserialize definitions from disk */
            using (StreamReader file = File.OpenText(Directory.GetCurrentDirectory() + "/definitions.json"))
            {
                parsedDefinitions = JsonConvert.DeserializeObject<ParsedDefinitions>(file.ReadToEnd());
            }

            /** Execute Builders */
            foreach (Definition definition in parsedDefinitions.definitions)
            {
                Builder builder = new Builder(db.GetConnection(), definition);

                builder.Run();
            }

            db.Disconnect();
        }
    }
}
