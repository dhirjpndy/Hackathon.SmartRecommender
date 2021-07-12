using ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection;
using System;

namespace ConsumerMarketplace.AdminFlexTool.Data.Contracts.DatabaseConnection.Factories
{
    /// <summary>
    /// Factory to create connection strings from various inputs
    /// </summary>
    public interface IConnectionStringFactory
    {

        /// <summary>
        /// Returns the correct connection string format for a subscriber using a default (App) username and password
        /// </summary>
        /// <param name="server"></param>
        /// <param name="databaseName"></param>
        /// <param name="connectionConfig"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">AppConnectionStringUser or AppConnectionStringPassword not found in config</exception>
        string CreateSubscriber(string server, string databaseName, ConnectionConfig connectionConfig);

        /// <summary>
        /// Returns the correct connection string format for a subscriber using a default (App) username and password
        /// </summary>
        /// <param name="server"></param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">AppConnectionStringUser or AppConnectionStringPassword not found in config</exception>
        string CreateSubscriber(string server, string databaseName);

        /// <summary>
        /// Returns the correct connection string format using the supplied information
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <param name="server">The server connecting to</param>
        /// <param name="databaseName">The database on the server connecting to</param>
        /// <returns></returns>
        string Create(string username, string password, string server, string databaseName);

        /// <summary>
        /// Returns the correct connection string from a supplied connection string store
        /// </summary>
        /// <param name="connectionStringName">Comes from web.config or other config file, example wsMaster</param>
        /// <returns></returns>
        /// <remarks>
        /// Connection string store should be bound to ConnectionStringSettingsCollection if using IoC.
        /// Default store is *.config connection strings section
        /// </remarks>
        /// <exception cref="ArgumentException">connectionStringName not found</exception>
        string Create(string connectionStringName);

        /// <summary>
        /// Returns the correct connection string from a supplied connection string store
        /// </summary>
        /// <param name="connectionStringName">Comes from web.config or other config file, example wsMaster</param>
        /// <param name="connectionConfig"></param>
        /// <returns></returns>
        /// <remarks>
        /// Connection string store should be bound to ConnectionStringSettingsCollection if using IoC.
        /// Default store is *.config connection strings section
        /// </remarks>
        /// <exception cref="ArgumentException">connectionStringName not found</exception>
        string Create(string connectionStringName, ConnectionConfig connectionConfig);

        /// <summary>
        /// Uses the connection string provided and overrides values based on connection configuration
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="connectionConfig"></param>
        /// <returns></returns>
        string HandleOverrides(string connectionString, ConnectionConfig connectionConfig);
    }
}
