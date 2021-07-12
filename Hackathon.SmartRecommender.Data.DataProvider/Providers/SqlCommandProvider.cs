using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Providers;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ConsumerMarketplace.AdminFlexTool.Data.Providers
{
    /// <summary>
    /// Implementation of ISqlCommand
    /// </summary>
    public class SqlCommandProvider : ISqlCommand
    {
        private SqlCommand _sqlCommand;

        /// <inheritdoc />
        public void SetSqlCommand(SqlCommand sqlCommand)
        {
            _sqlCommand = sqlCommand;
        }

        /// <inheritdoc />
        public int ExecuteNonQuery()
        {
            try
            {
                return _sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc />
        public ISqlDataReader ExecuteReader()
        {
            try
            {
                return new SqlDataReaderProvider(_sqlCommand.ExecuteReader());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc />
        public async Task<ISqlDataReader> ExecuteReaderAsync()
        {
            try
            {
                return new SqlDataReaderProvider(await _sqlCommand.ExecuteReaderAsync());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc />
        public async Task<int> ExecuteNonQueryAsync()
        {
            try
            {
                return await _sqlCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc />
        public object ExecuteScalar()
        {
            try
            {
                return _sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc />
        public async Task<object> ExecuteScalarAsync()
        {
            try
            {
                return await _sqlCommand.ExecuteScalarAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
