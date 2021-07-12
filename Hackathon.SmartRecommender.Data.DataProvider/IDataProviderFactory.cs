using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Repositories;
using ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection;
using System;

namespace Hackathon.SmartRecommender.Data
{
    /// <summary>
    ///     Primary method for usage of an IDataProvider
    /// </summary>
    public interface IDataProviderFactory
    {
        /// <summary>
        ///     Creates IDataProvider for the connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">connectionString</exception>
        IDataProvider CreateForConnectionString(string connectionString);

        IDataProvider CreateForConnectionStringName(string connectionName); 

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
        ///     Creates IDataProvider for the specific subscriber
        /// </summary>
        /// <param name="subscriberId">the subscriber</param>
        /// <returns>A data provider for the requested subscriber</returns>
        /// <remarks>
        ///		SubscriberProviderEnableCaching = true in configuration to enable caching
        ///     Requires configuration settings: AppConnectionStringUser, AppConnectionStringPassword
        /// </remarks>
        /// <exception cref="SubscriberNotAvailableException">subscriber not found</exception>
        /// <exception cref="InvalidOperationException">
        ///     AppConnectionStringUser or AppConnectionStringPassword not found
        /// </exception>
        IDataProvider CreateForSubscriber(int subscriberId);
    }
}