using System;
using System.Data;
using MySqlConnector;
using CLIHelpers;

namespace dbc_export
{
    class Database
    {
        /** Connection Details */
        private string host;
        private int port;
        private string user;
        private string pass;
        private string world;

        private MySqlConnection connection;

        /// <summary>
        /// Build an instance of the Database Connection Handler.
        /// </summary>
        /// <param name="host">The hostname / IP we should use.</param>
        /// <param name="port">The port the database is active on.</param>
        /// <param name="user">The username to use.</param>
        /// <param name="pass">The password to use.</param>
        /// <param name="world">The name of the Azeroth-Core world database.</param>
        public Database(string host, int port, string user, string pass, string world)
        {
            this.host = host;
            this.port = port;
            this.user = user;
            this.pass = pass;
            this.world = world;
        }

        /// <summary>
        /// Begins the process of connecting to the MySQL Database.
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            string credentials = String.Format(
                "Server={0};Port={1};UID={2};Pwd={3};Database={4}",
                this.host,
                this.port,
                this.user,
                this.pass,
                this.world
            );

            Logger.Info(String.Format("Attempting Connection with Details - {0}\n", credentials));

            this.connection = new MySqlConnection(credentials);

            try
            {
                this.connection.Open();
            }
            catch (MySqlConnector.MySqlException)
            {
                return false;
            }

            return connection.State == ConnectionState.Open;
        }

        /// <summary>
        /// Get the active MySQL Connection instance.
        /// </summary>
        /// <returns>Connection.</returns>
        public MySqlConnection GetConnection()
        {
            return this.connection;
        }

        /// <summary>
        /// Simply Disconnects the current MySQL Connection Instance.
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            this.connection.Dispose();

            return this.connection.State == ConnectionState.Closed;
        }
    }
}