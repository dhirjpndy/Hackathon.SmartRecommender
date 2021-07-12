using ConsumerMarketplace.AdminFlexTool.Data.Contracts.DatabaseConnection.Factories;
using ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection;
using ConsumerMarketplace.AdminFlexTool.Data.UnitOfWork;

namespace ConsumerMarketplace.AdminFlexTool.Data.SubscriberInfo.DbContexts
{
    internal class SubscriberContext : DbContext
    {
        private readonly IConnectionStringFactory _connectionStringFactory;
        private readonly SubscriberDatabase _subscriberDatabase;

        public SubscriberContext(
            IDbConnectionFactory dbConnectionFactory,
            IConnectionStringFactory connectionStringFactory,
            SubscriberDatabase subscriberDatabase,
            ConnectionConfig scope
        )
            : base(dbConnectionFactory, scope)
        {
            _connectionStringFactory = connectionStringFactory;
            _subscriberDatabase = subscriberDatabase;
        }

        /// <summary>
        /// The connection string this dbContext will use
        /// </summary>
        public override string ConnectionString => _connectionStringFactory.CreateSubscriber(
            _subscriberDatabase.ServerName,
            _subscriberDatabase.DatabaseName,
            ConnectionConfig);

        /// <summary>
        /// The database this dbContext will use
        /// </summary>
        public override string Database => _subscriberDatabase.DatabaseName;
    }
}