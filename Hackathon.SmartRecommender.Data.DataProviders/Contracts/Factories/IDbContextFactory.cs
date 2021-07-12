using ConsumerMarketplace.AdminFlexTool.Data.Contracts.UnitOfWork;
using ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection;
using ConsumerMarketplace.AdminFlexTool.Data.Exceptions;
using System;

namespace ConsumerMarketplace.AdminFlexTool.Data.Contracts.Factories
{
    /// <summary>
    /// Provides a factory for creating DbContexts for our common databases
    /// </summary>
    public interface IDbContextFactory
    {
        /// <summary>
        /// Creates a context for WsMaster, requires a connection string be defined in this application's config as: wsMaster
        /// </summary>
        /// <param name="connectionConfig"></param>
        /// <returns></returns>
        /// <exception cref="SubscriberNotAvailableException">wsMaster connection string not found</exception>
        IDbContext CreateForWsMaster(ConnectionConfig connectionConfig);

        /// <summary>
        /// Creates a context for the connection string in the configured collection
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <param name="connectionConfig"></param>
        /// <returns></returns>
        /// <exception cref="SubscriberNotAvailableException">connectionStringName does not exist</exception>
        IDbContext CreateByName(string connectionStringName, ConnectionConfig connectionConfig);

        /// <summary>
        /// Creates a context for the specified connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="connectionConfig"></param>
        /// <returns></returns>
        /// <exception cref="SubscriberNotAvailableException">connection string is null</exception>
        IDbContext Create(string connectionString, ConnectionConfig connectionConfig);

        /// <summary>
        /// Creates a context for the subscriber database
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <param name="databaseName"></param>
        /// <param name="databaseServerName"></param>
        /// <param name="connectionConfig"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">databaseName, databaseServerName</exception>
        IDbContext CreateForSubscriber(int subscriberId, string databaseName, string databaseServerName, ConnectionConfig connectionConfig);
    }
}