
using Microsoft.Extensions.Logging;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Engines;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Factories;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Providers;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Repositories;
using ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection;
using ConsumerMarketplace.AdminFlexTool.Data.Exceptions;
using Hackathon.SmartRecommender.Data;

namespace ConsumerMarketplace.AdminFlexTool.Data.Factories
{
    /// <inheritdoc cref="IDataProviderFactory" />
    internal class DataProviderFactory : CoreDataProviderFactory, IDataProviderFactory
    {
        private ISubscriberDbProvider _subscriberProvider;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContextFactory"></param>
        /// <param name="sqlEngine"></param>
        /// <param name="subscriberProvider"></param>
        /// <param name="sqlCommand"></param>
        /// <param name="logger"></param>
        public DataProviderFactory(
            IDbContextFactory dbContextFactory,
            ISqlEngine sqlEngine,
            ISubscriberDbProvider subscriberProvider,
            ISqlCommand sqlCommand,
            ILogger<IDataProvider> logger )
            : base(dbContextFactory, sqlEngine, sqlCommand, logger)
        {
            _subscriberProvider = subscriberProvider;
        }

        /// <inheritdoc />
        public void SetSubscriberDbProvider(ISubscriberDbProvider subscriberDbProvider)
        {
            _subscriberProvider = subscriberDbProvider;
        }

        /// <inheritdoc />
        public IDataProvider CreateForSubscriber(int subscriberId)
        {
            return CreateForSubscriber(subscriberId, null);
        }

        /// <inheritdoc />
        public IDataProvider CreateForSubscriber(int subscriberId, ConnectionConfig scope)
        {
            var subscriberDbInfo = _subscriberProvider.GetSubscriberDatabaseInfo(subscriberId);
            if (subscriberDbInfo == null)
            {
                throw new SubscriberNotAvailableException(subscriberId);
            }

            return CreateForContext(DbContextFactory.CreateForSubscriber(
                subscriberId,
                subscriberDbInfo.DatabaseName,
                subscriberDbInfo.ServerName,
                scope));
        }
    }
}