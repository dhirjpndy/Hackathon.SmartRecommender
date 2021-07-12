using ConsumerMarketplace.AdminFlexTool.Data.Contracts.DatabaseConnection.Factories;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.UnitOfWork;
using ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection;
using System;

namespace ConsumerMarketplace.AdminFlexTool.Data.UnitOfWork
{
    /// <summary>
    /// An abstract implementation of IDbContext.  Implement this abstract class by defining a ConnectionString
    /// for your project and pass that Implementation as the generic parameter to IDataProvider
    /// </summary>
    /// <inheritdoc />
    public abstract class DbContext : IDbContext
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        /// <summary>
        /// Instantiates the necessary tools for a db context to be able to create its connections
        /// </summary>
        /// <param name="dbConnectionFactory"></param>
        /// <param name="scope"></param>
        /// <exception cref="ArgumentNullException">dbConnectionFactory, dbFactory</exception>
        protected DbContext(IDbConnectionFactory dbConnectionFactory, ConnectionConfig scope)
        {
            _dbConnectionFactory = dbConnectionFactory;
            ConnectionConfig = scope;
        }

        /// <inheritdoc />
        public IDb Db { get; set; }

        /// <inheritdoc />
        public abstract string ConnectionString { get; }

        /// <inheritdoc cref="IDbContext" />
        public abstract string Database { get; }

        /// <inheritdoc />
        public virtual IDb CreateAndStore()
        {
            Db = new Db
            {
                UseDatabase = Database,
                Connection = _dbConnectionFactory.Create(ConnectionString, Database)
            };

            return Db;
        }

        /// <summary>
        /// Sets data provider specific configuration information which affects items such as connection string
        /// </summary>
        internal ConnectionConfig ConnectionConfig { get; private set; }
    }
}