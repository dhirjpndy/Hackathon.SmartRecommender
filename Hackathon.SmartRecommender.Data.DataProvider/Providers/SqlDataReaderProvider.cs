using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Providers;
using System.Data;
using System.Data.SqlClient;

namespace ConsumerMarketplace.AdminFlexTool.Data.Providers
{
    /// <summary>
    /// Implementatoin of ISqlDataReader
    /// </summary>
    public class SqlDataReaderProvider : ISqlDataReader
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sqlDataReader"></param>
        public SqlDataReaderProvider(SqlDataReader sqlDataReader)
        {
            SqlDataReader = sqlDataReader;
        }

        /// <inheritdoc />
        public IDataReader SqlDataReader { get; set; }

        /// <inheritdoc />
        public bool IsClosed => this.SqlDataReader.IsClosed;
    }
}
