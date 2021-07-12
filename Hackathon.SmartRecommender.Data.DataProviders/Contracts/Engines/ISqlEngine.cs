using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Providers;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ConsumerMarketplace.AdminFlexTool.Data.Contracts.Engines
{
    /// <summary>
    /// Internal engine for sql commands
    /// </summary>
    public interface ISqlEngine
    {
        /// <summary>
        ///     executes a data reader against the specified command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        DataTable ExecuteReader(ISqlCommand command);

        /// <summary>
        /// Asynchronously executes a data reader against a specific command
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<DataTable> ExecuteReaderAsync(ISqlCommand query);


        /// <summary>
        /// Executes the reader asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> ExecuteReaderAsync<T>(ISqlCommand query, Func<IDataReader, T> mapper);

        /// <summary>
        /// Bulds the command with SQL, parameters, and options.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        SqlCommand BuildCommandWithSqlAndParameters(IDb db, string sql,
            List<SqlParameter> parameters, CommandOptions options = null);      
    }
}