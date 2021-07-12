using ConsumerMarketplace.AdminFlexTool.Data.Contracts.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ConsumerMarketplace.AdminFlexTool.Data.Contracts.Repositories
{
    /// <summary>
    /// Contains helper functions for executing command commands
    /// </summary>
    /// <inheritdoc />
    public interface IDataProvider : IDisposable
    {
        /// <summary>
        /// Get the current Db for the context
        /// </summary>
        /// <returns></returns>
        IDb GetDb();

        /// <summary>
        /// Executes and reads one value asynchronously
        /// </summary>
        /// <typeparam name="T">The type of the value that will be read</typeparam>
        /// <param name="command">The command to run</param>
        /// <param name="parameters">A list of named parameters</param>
        /// <param name="storedProcedure"></param>
        /// <returns>The first column of the first row in the result set</returns>
        Task<T> ExecuteScalarAsync<T>(string command, List<SqlParameter> parameters = null, bool storedProcedure = false);

        /// <summary>
        /// Executes and reads one value asynchronously
        /// </summary>
        /// <typeparam name="T">The type of the value that will be read</typeparam>
        /// <param name="command">The command to run</param>
        /// <param name="parameters">A list of named parameters</param>
        /// <returns>The of affected rows</returns>
        Task<T> ExecuteNonQueryAsync<T>(string command, List<SqlParameter> parameters = null);

        /// <summary>
        /// Reads collection of values
        /// </summary>
        /// <typeparam name="T">The type of the values that will be read</typeparam>
        /// <param name="command">The command to run</param>
        /// <param name="parameters">A list of named parameters</param>
        /// <param name="storedProcedure"></param>
        /// <returns>A list of T</returns>
        IEnumerable<T> Read<T>(string command, List<SqlParameter> parameters = null, bool storedProcedure = false);

        /// <summary>
        /// Reads collection of values using the func to convert them to T objects
        /// </summary>
        /// <typeparam name="T">The type of the values that will be read</typeparam>
        /// <param name="command">The command to run</param>
        /// <param name="mapper">A mapper that will convert the read data into objects of T</param>
        /// <param name="parameters">A list of named parameters</param>
        /// <param name="storedProcedure"></param>
        /// <returns>A list of T</returns>
        IEnumerable<T> Read<T>(string command, Func<DataRow, T> mapper, List<SqlParameter> parameters = null, bool storedProcedure = false);

        /// <summary>
        /// Reads async collection of values using the func to convert them to T objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="mapper"></param>
        /// <param name="parameters"></param>
        /// <param name="storedProcedure"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> ReadAsync<T>(string command, Func<IDataReader, T> mapper, List<SqlParameter> parameters = null,
            bool storedProcedure = false);

        /// <summary>
        /// Reads async collection of values using the func to convert them to T objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="mapper"></param>
        /// <param name="options"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> ReadAsync<T>(string command, Func<IDataReader, T> mapper, CommandOptions options,
            List<SqlParameter> parameters = null);
       
        /// <summary>
        /// Disposes and closes connection.
        /// </summary>
        void DisposeAndClose();
    }
}
