using ConsumerMarketplace.AdminFlexTool.Data.Contracts.DatabaseConnection.Factories;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Providers;
using ConsumerMarketplace.AdminFlexTool.Data.Engines;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection
{
    /// <summary>
    /// Connection Factory
    /// </summary>
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private ISqlConnection _sqlConnection;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sqlConnection"></param>
        public DbConnectionFactory(ISqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        private IDbConnection CreateOpen(string connectionString, string database)
        {
            var connection = _sqlConnection.Create(connectionString);

            //swapped from .equals because of CLR Heap Allocations because of 
            //not overriding equals on ConnectionState enum
            if (connection.State != ConnectionState.Open)
            {
                try
                {
                    Debug.WriteLine("Connection Created & Opened for: {0}", (object)connectionString);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                connection.Open();
            }
            else
            {
                try
                {
                    Debug.WriteLine("Connection Recreated for: {0}", (object)connectionString);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (ConnectionStringInformation.InlineUse || string.IsNullOrWhiteSpace(database))
            {
                return connection.SqlConnection;
            }

            var useStatement = SqlEngine.CreateEscapedUseDatabaseStatement(database);
            try
            {
                Debug.WriteLine(">>> DB Set Command:" + useStatement);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var sqlCommand = new SqlCommand
            {
                Connection = connection.SqlConnection as SqlConnection,
                CommandText = useStatement
            };
            sqlCommand.ExecuteNonQuery();

            return connection.SqlConnection;
        }

        #region IDbConnectionFactory Members

        /// <inheritdoc />
        public IDbConnection Create(string connectionString, string database)
        {
            try
            {
                return CreateOpen(connectionString, database);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        #endregion
    }
}