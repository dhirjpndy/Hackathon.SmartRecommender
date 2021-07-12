using Microsoft.Extensions.DependencyInjection;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.DatabaseConnection.Factories;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Engines;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Factories;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Providers;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Repositories;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.UnitOfWork;
using ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection;
using ConsumerMarketplace.AdminFlexTool.Data.Engines;
using ConsumerMarketplace.AdminFlexTool.Data.Factories;
using ConsumerMarketplace.AdminFlexTool.Data.Providers;
using ConsumerMarketplace.AdminFlexTool.Data.Repositories;
using ConsumerMarketplace.AdminFlexTool.Data.Settings;
using ConsumerMarketplace.AdminFlexTool.Data.UnitOfWork;

namespace ConsumerMarketplace.AdminFlexTool.Data
{
    /// <summary>
    /// Public entry point to bootstrap DataProviders functionality
    /// </summary>
    public static class Binder
    {
        /// <summary>
        /// Registration entry point
        /// </summary>
        /// <param name="services"></param>        
        /// <returns></returns>
        public static IServiceCollection RegisterDataProviders(this IServiceCollection services)
        {
            services.AddTransient<ISqlConnection, SqlConnectionProvider>();
            services.AddTransient<ISqlCommand, SqlCommandProvider>();
            services.AddTransient<ISqlDataReader, SqlDataReaderProvider>();
            services.AddTransient<IDb, Db>();
            services.AddTransient<ISqlEngine, SqlEngine>();
            services.AddTransient<ISubscriberDbProvider, SubscriberDbSqlRepository>();
            services.AddTransient<ICoreDataProviderFactory, CoreDataProviderFactory>();
            services.AddTransient<IDataProviderFactory, DataProviderFactory>();
            services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();
            services.AddTransient<IConnectionStringFactory, ConnectionStringFactory>();
            services.AddTransient<IDbContextFactory, DbContextFactory>();
            return services;
        }

        /// <summary>
        /// Entry point for Startup.cs of consuming applications
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dataProviderSettings"></param>
        /// <param name="connectionStringsSettings"></param>
        public static void AddDataProvider(this IServiceCollection services, DataProviderSettings dataProviderSettings, ConnectionStringsSettings connectionStringsSettings)
        {
            AddDataProvider(dataProviderSettings, connectionStringsSettings);
        }

        /// <summary>
        /// Entry point for Startup.cs of consuming applications
        /// </summary>
        /// <param name="dataProviderSettings"></param>
        /// <param name="connectionStringsSettings"></param>
        public static void AddDataProvider(DataProviderSettings dataProviderSettings, ConnectionStringsSettings connectionStringsSettings)
        {
            ConnectionStringInformation.Init(dataProviderSettings);
            ConnectionStringFactory.Init(connectionStringsSettings);
        }
    }
}
