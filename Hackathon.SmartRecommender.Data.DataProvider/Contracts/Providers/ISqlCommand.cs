using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ConsumerMarketplace.AdminFlexTool.Data.Contracts.Providers
{
    /// <summary>
    /// Interface so we can use SqlCommand with DI
    /// </summary>
    public interface ISqlCommand
    {
        /// <summary>
        /// Set the SqlCommand behind the interface
        /// </summary>
        /// <param name="sqlCommand"></param>
        void SetSqlCommand(SqlCommand sqlCommand);

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <returns></returns>
        ISqlDataReader ExecuteReader();

        /// <summary>
        /// ExecuteReaderAsync
        /// </summary>
        /// <returns></returns>
        Task<ISqlDataReader> ExecuteReaderAsync();

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <returns></returns>
        int ExecuteNonQuery();

        /// <summary>
        /// ExecuteNonQueryAsync
        /// </summary>
        /// <returns></returns>
        Task<int> ExecuteNonQueryAsync();

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <returns></returns>
        object ExecuteScalar();

        /// <summary>
        /// ExecuteScalarAsync
        /// </summary>
        /// <returns></returns>
        Task<object> ExecuteScalarAsync();
    }
}
