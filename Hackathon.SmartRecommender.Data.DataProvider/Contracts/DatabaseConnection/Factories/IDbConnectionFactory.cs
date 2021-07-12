using System.Data;

namespace ConsumerMarketplace.AdminFlexTool.Data.Contracts.DatabaseConnection.Factories
{
    /// <summary>
    /// Factory for IDbConnections
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Creates an IDbConnection from a connection string
        /// </summary>
        /// <param name="connectionString">The connection string to use for this IDbConnection</param>
        /// <param name="database"></param>
        /// <returns>The IDbConnection</returns>
        IDbConnection Create(string connectionString, string database);
    }
}
