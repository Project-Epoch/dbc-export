using System;
using System.Data;
using System.IO;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

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

        public Database(string host, int port, string user, string pass, string world)
        {
            this.host = host;
            this.port = port;
            this.user = user;
            this.pass = pass;
            this.world = world;
        }

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

            Console.WriteLine(String.Format("Attempting Connection with Details - {0}", credentials));

            this.connection = new MySqlConnection(credentials);
            this.connection.Open();

            return connection.State == ConnectionState.Open;
        }

        public MySqlConnection GetConnection()
        {
            return this.connection;
        }

        public bool Disconnect()
        {
            this.connection.Dispose();

            return this.connection.State == ConnectionState.Closed;
        }
    }
}