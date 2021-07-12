using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Repositories;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.UnitOfWork;
using ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection;
using System;

namespace ConsumerMarketplace.AdminFlexTool.Data
{
    /// <summary>
    ///     Primary method for usage of an IDataProvider
    /// </summary>
    /// <remarks>This is a subset of <see cref="IDataProviderFactory"/> to break dependency on any subscriber DB</remarks>
    public interface ICoreDataProviderFactory
    {
        /// <summary>
        ///     Creates IDataProvider for the specific context
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">dbContext</exception>
        IDataProvider CreateForContext(IDbContext dbContext);

        /// <summary>
        ///     Creates IDataProvider for wsMaster
        ///     This requires an application configuration with the following connection string value: wsMaster
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">wsMaster configuration value not found</exception>
        IDataProvider CreateForWsMaster();

        /// <summary>
        ///     Creates IDataProvider for wsMaster
        ///     This requires an application configuration with the following connection string value: wsMaster
        /// </summary>
        /// <param name="scope"><see cref="ConnectionFlags"/> can be used in place of ConnectionConfig</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">wsMaster configuration value not found</exception>
        IDataProvider CreateForWsMaster(ConnectionConfig scope);

        /// <summary>
        ///     Creates IDataProvider for the connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">connectionString</exception>
        IDataProvider CreateForConnectionString(string connectionString);

        /// <summary>
        ///     Creates IDataProvider for the connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="scope"><see cref="ConnectionFlags"/> can be used in place of ConnectionConfig</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">connectionString</exception>
        IDataProvider CreateForConnectionString(string connectionString, ConnectionConfig scope);

        /// <summary>
        ///     Creates an IDataProvider for the connection string name as found in the current application's configuration file
        /// </summary>
        /// <param name="connectionStringName">the name of the connection string</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">connectionStringName not found</exception>
        IDataProvider CreateForConnectionStringName(string connectionStringName);

        /// <summary>
        ///     Creates an IDataProvider for the connection string name as found in the current application's configuration file
        /// </summary>
        /// <param name="connectionStringName">the name of the connection string</param>
        /// <param name="scope"><see cref="ConnectionFlags"/> can be used in place of ConnectionConfig</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">connectionStringName not found</exception>
        IDataProvider CreateForConnectionStringName(string connectionStringName, ConnectionConfig scope);
    }
}
