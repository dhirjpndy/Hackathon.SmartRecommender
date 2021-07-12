using Microsoft.Extensions.Logging;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Engines;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Providers;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Repositories;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.UnitOfWork;
using ConsumerMarketplace.AdminFlexTool.Data.Factories;
using ConsumerMarketplace.AdminFlexTool.Reports.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataRow = System.Data.DataRow;

namespace ConsumerMarketplace.AdminFlexTool.Data.Repositories
{
    internal class SqlDataProvider : IDataProvider
    {
        private readonly IDbContext _dbContext;
        private readonly ISqlEngine _sqlEngine;
        private readonly ISqlCommand _sqlCommand;
        private int _usageCount;
        private readonly ILogger<IDataProvider> _logger;

        public SqlDataProvider(IDbContext dbContext, ISqlEngine sqlEngine, ISqlCommand sqlCommand, ILogger<IDataProvider> logger)
        {
            _dbContext = dbContext;
            _sqlEngine = sqlEngine;
            _sqlCommand = sqlCommand;
            _logger = logger;
        }

        /// <inheritdoc />
        public IDb GetDb()
        {
            if (_dbContext.Db != null && _dbContext.Db.Connection?.State == ConnectionState.Open)
            {
                return _dbContext.Db;
            }
            if (_dbContext.Db != null && _dbContext.Db.Connection != null)
            {
                _dbContext.Db.Connection.Dispose();
            }

            return _dbContext.CreateAndStore();
        }

        #region AsyncReads

        public async Task<IEnumerable<T>> ReadAsync<T>(string command, Func<IDataReader, T> mapper, List<SqlParameter> parameters = null, bool storedProcedure = false)
        {
            return await DoAndReportError(async db =>
            {
                var options = Constants.CommandOptionForStoredProcFlag(storedProcedure);
                _sqlCommand.SetSqlCommand(_sqlEngine.BuildCommandWithSqlAndParameters(db, command, parameters, options));
                return (await _sqlEngine.ExecuteReaderAsync(_sqlCommand, mapper).ConfigureAwait(false));

            }).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> ReadAsync<T>(string command, Func<IDataReader, T> mapper, CommandOptions options, List<SqlParameter> parameters = null)
        {
            return await DoAndReportError(async db =>
            {
                _sqlCommand.SetSqlCommand(_sqlEngine.BuildCommandWithSqlAndParameters(db, command, parameters, options));
                return (await _sqlEngine.ExecuteReaderAsync(_sqlCommand, mapper).ConfigureAwait(false));

            }).ConfigureAwait(false);
        }

        #endregion

        #region Error Handling and Reporting

        private TResult DoAndReportError<TResult>(Func<IDb, TResult> lambda)
        {
            ++_usageCount;

            try
            {
                var db = GetDb();
                return lambda(db);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Exception Occured In Sql Data Provider: " + ex.Message);
                throw;
            }
        }

        private void ReportUsageInfoOnCleanup()
        {
            if (_usageCount == 0)
            {
                //DataProviderTracer.ReportUnusedProvider();
            }

            // TODO add more based on methods and persisted state
        }

        #endregion

        #region Cleanup and End-of-action

        /// <inheritdoc />
        public void DisposeAndClose()
        {
            ReportUsageInfoOnCleanup();

            var db = _dbContext.Db;
            _dbContext.Db = null;

            if (db?.Connection?.State != ConnectionState.Open)
            {
                return;
            }

            db.Transaction?.Commit();

            try
            {
                Debug.WriteLine("Closing {0}", _dbContext.ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            db.Connection.Close();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            DisposeAndClose();
        }


        public async Task<T> ExecuteScalarAsync<T>(string command, List<SqlParameter> parameters = null, bool storedProcedure = false)
        {
            return await DoAndReportError(async db =>
            {
                var options = Constants.CommandOptionForStoredProcFlag(storedProcedure);
                _sqlCommand.SetSqlCommand(_sqlEngine.BuildCommandWithSqlAndParameters(db, command, parameters, options));
                var result = await _sqlCommand.ExecuteScalarAsync().ConfigureAwait(false);
                return result.GetValue<T>();
            }).ConfigureAwait(false);
        }

        public async Task<T> ExecuteNonQueryAsync<T>(string command, List<SqlParameter> parameters = null)
        {
            return await DoAndReportError(async db =>
            {
                _sqlCommand.SetSqlCommand(_sqlEngine.BuildCommandWithSqlAndParameters(db, command, parameters));
                var result = await _sqlCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                return result.GetValue<T>();
            }).ConfigureAwait(false);
        }

        public IEnumerable<T> Read<T>(string command, List<SqlParameter> parameters = null, bool storedProcedure = false)
        {
            return DoAndReportError(db =>
            {
                var options = Constants.CommandOptionForStoredProcFlag(storedProcedure);
                _sqlCommand.SetSqlCommand(_sqlEngine.BuildCommandWithSqlAndParameters(db, command, parameters, options));
                var list = _sqlEngine.ExecuteReader(_sqlCommand).AsEnumerable().Select(result => result[0]);
                return list.Select(result => result.GetValue<T>());
            });
        }

        public IEnumerable<T> Read<T>(string command, Func<DataRow, T> mapper, List<SqlParameter> parameters = null, bool storedProcedure = false)
        {
            return DoAndReportError(db =>
            {
                var options = Constants.CommandOptionForStoredProcFlag(storedProcedure);
                _sqlCommand.SetSqlCommand(_sqlEngine.BuildCommandWithSqlAndParameters(db, command, parameters, options));
                return _sqlEngine.ExecuteReader(_sqlCommand).AsEnumerable().Select(mapper);
            });
        }

        public Task<IEnumerable<T>> ReadAsync<T>(string command, Func<DataRow, T> mapper, CommandOptions options, List<SqlParameter> parameters = null)
        {
            throw new NotImplementedException();
        }



        #endregion
    }
}
