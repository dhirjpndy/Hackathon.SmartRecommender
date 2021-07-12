using System.Data;

namespace ConsumerMarketplace.AdminFlexTool.Data.Contracts.Providers
{
    /// <summary>
    /// Interface for SqlConnection so we can use DI
    /// </summary>
    public interface ISqlConnection
    {
        /// <summary>
        /// Get a IDbConnection frmo the ISqlConnection
        /// </summary>
        IDbConnection SqlConnection { get; set; }

        /// <summary>
        /// ConnectionString
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Get the State
        /// </summary>
        ConnectionState State { get; }

        /// <summary>
        /// Open
        /// </summary>
        void Open();

        /// <summary>
        /// Close
        /// </summary>
        void Close();

        /// <summary>
        /// Create and return a connection to this
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        ISqlConnection Create(string connection);

        /// <summary>
        /// The Database
        /// </summary>
        string Database { get; }
    }
}
