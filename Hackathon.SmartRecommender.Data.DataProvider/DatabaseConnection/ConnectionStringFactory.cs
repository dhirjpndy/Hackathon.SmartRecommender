using ConsumerMarketplace.AdminFlexTool.Data.Contracts.DatabaseConnection.Factories;
using ConsumerMarketplace.AdminFlexTool.Data.Settings;
using System;
using System.Data.SqlClient;

namespace ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection
{
    internal class ConnectionStringFactory : IConnectionStringFactory
    {
        /// <summary>
        ///     Encourage reuse of parameter instead of having to return to garbage collection
        /// </summary>
        private static readonly ConnectionConfig DefaultConfigurationConfig = new ConnectionConfig
        {
            UserName = ConnectionStringInformation.AppUserName,
            Password = ConnectionStringInformation.AppPassword
        };

        private static readonly string PoolingString;
        private static ConnectionStringsSettings ConnectionStrings;

        public static void Init(ConnectionStringsSettings connectionStringsSettings)
        {
            ConnectionStrings = connectionStringsSettings;
        }

        static ConnectionStringFactory()
        {
            const string poolingFalse = ";Pooling=false";
            PoolingString = ConnectionStringInformation.MinPoolingOrNegForDisable < 0
                ? poolingFalse
                : ";Min Pool Size=" + ConnectionStringInformation.MinPoolingOrNegForDisable;

            if (ConnectionStringInformation.MaxPoolSize != ConnectionStringInformation.SqlMaxPoolSystemDefault)
            {
                PoolingString += ";Max Pool Size=" + ConnectionStringInformation.MaxPoolSize;
            }

            PoolingString += ";MultipleActiveResultSets=" +
                             ConnectionStringInformation.EnabledMars.ToString().ToLower();

            PoolingString += ";Application Name=" + ConnectionStringInformation.AppName;

            if (ConnectionStringInformation.ConnectionTimeout !=
                ConnectionStringInformation.SqlConnectionTimeoutSystemDefault)
            {
                PoolingString += ";Connection Timeout=" + ConnectionStringInformation.ConnectionTimeout;
            }
        }

        private static bool HasSpecialConnectionStringSettings(ConnectionConfig connectionConfig)
        {
            if (connectionConfig == null) return false;

            return connectionConfig.Flags != ConnectionFlags.Default
                   || !string.IsNullOrWhiteSpace(connectionConfig.UserName)
                   || !string.IsNullOrWhiteSpace(connectionConfig.Password);
        }

        private static string OverwriteConnectionConfigOptions(string connectionString,
            ConnectionConfig connectionConfig)
        {
            if (!HasSpecialConnectionStringSettings(connectionConfig))
            {
                return connectionString;
            }

            var stringBuilder = new SqlConnectionStringBuilder(connectionString);

            if (connectionConfig.DisablePooling)
            {
                stringBuilder.Pooling = false;
                stringBuilder.MinPoolSize = 0;
            }

            if (connectionConfig.ReadOnly)
            {
                stringBuilder.ApplicationIntent = ApplicationIntent.ReadOnly;
            }

            if (connectionConfig.EnableMars)
            {
                stringBuilder.MultipleActiveResultSets = true;
            }

            if (!string.IsNullOrWhiteSpace(connectionConfig.UserName))
            {
                stringBuilder.UserID = connectionConfig.UserName;
            }

            if (!string.IsNullOrWhiteSpace(connectionConfig.Password))
            {
                stringBuilder.Password = connectionConfig.Password;
            }

            return stringBuilder.ToString();
        }

        #region IConnectionStringFactory Members

        public string CreateSubscriber(string server, string databaseName, ConnectionConfig connectionConfig)
        {
            if (connectionConfig == null)
            {
                connectionConfig = DefaultConfigurationConfig;
            }
            var username = connectionConfig.UserName ?? ConnectionStringInformation.AppUserName;
            var password = connectionConfig.Password ?? ConnectionStringInformation.AppPassword;

            if (ConnectionStringInformation.MinPoolingOrNegForDisable >= 0 && !connectionConfig.DisablePooling)
            {
                // Note this will be set via SqlEngine via a USE statement as required
                databaseName = null;
            }

            var databaseString = string.IsNullOrWhiteSpace(databaseName)
                ? string.Empty
                : $";Database={databaseName.Trim()}";

            var connectionString =
                $"Server=tcp:{server.Trim()};User ID={username};Password={password}{databaseString}{PoolingString}";

            return OverwriteConnectionConfigOptions(connectionString, connectionConfig);
        }

        /// <inheritdoc />
        public string CreateSubscriber(string server, string databaseName)
        {
            return CreateSubscriber(server, databaseName, null);
        }

        /// <inheritdoc />
        public string Create(string username, string password, string server, string databaseName)
        {
            return CreateSubscriber(server, databaseName, new ConnectionConfig
            {
                UserName = username,
                Password = password
            });
        }

        /// <inheritdoc />
        public string Create(string connectionStringName)
        {
            string connectionString = string.Empty;
            ConnectionStrings.ConnectionStrings.TryGetValue(connectionStringName, out connectionString);
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                return connectionString;
            }

            //if we reached here then the connection string wasn't present
            throw new ArgumentException("Invalid connection string request for " + connectionStringName);
        }

        /// <inheritdoc />
        public string Create(string connectionStringName, ConnectionConfig connectionConfig)
        {
            var connectionString = Create(connectionStringName);
            return OverwriteConnectionConfigOptions(connectionString, connectionConfig);
        }

        /// <inheritdoc />
        public string HandleOverrides(string connectionString, ConnectionConfig connectionConfig)
        {
            return OverwriteConnectionConfigOptions(connectionString, connectionConfig);
        }

        #endregion
    }
}