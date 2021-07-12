using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Providers;
using System.Data;
using System.Data.SqlClient;

namespace ConsumerMarketplace.AdminFlexTool.Data.Providers
{
    /// <summary>
    /// Implementation of ISqlConnection
    /// </summary>
    public class SqlConnectionProvider : ISqlConnection
    {
        /// <inheritdoc />
        public IDbConnection SqlConnection { get; set; }

        /// <inheritdoc />
        public ConnectionState State { get { return SqlConnection.State; } }

        /// <inheritdoc />
        public string Database => SqlConnection.Database;

        /// <inheritdoc />
        public string ConnectionString => SqlConnection.ConnectionString;

        /// <inheritdoc />
        public void Close()
        {
            SqlConnection.Close();
        }

        /// <inheritdoc />
        public ISqlConnection Create(string connection)
        {
            SqlConnection = new SqlConnection(connection);
            return this;
        }

        /// <inheritdoc />
        public void Open()
        {
            SqlConnection.Open();
        }
    }
}
