using Microsoft.Extensions.Logging;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Engines;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.Providers;
using ConsumerMarketplace.AdminFlexTool.Data.Contracts.UnitOfWork;
using ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection;
using ConsumerMarketplace.AdminFlexTool.Data.Factories;
using ConsumerMarketplace.AdminFlexTool.Reports.Data.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsumerMarketplace.AdminFlexTool.Data.Engines
{
    internal class SqlEngine : ISqlEngine
    {
        private ILogger<SqlEngine> _logger;
        public SqlEngine(ILogger<SqlEngine> logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     executes a data reader against the specified command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataTable ExecuteReader(ISqlCommand command)
        {
            try
            {
                var table = new DataTable();
                var reader = command.ExecuteReader();
                table.Load(reader.SqlDataReader);
                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> ExecuteReaderAsync(ISqlCommand query)
        {
            try
            {
                var table = new DataTable();
                var reader = await query.ExecuteReaderAsync().ConfigureAwait(false);
                table.Load(reader.SqlDataReader);
                return table;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occured In Sql Engine -ExecuteReaderAsync: " + ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<T>> ExecuteReaderAsync<T>(ISqlCommand query, Func<IDataReader, T> mapper)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();
                _logger.LogInformation("Start ExecuteReaderAsync for :" + typeof(T).Name);
                var reader = await query.ExecuteReaderAsync().ConfigureAwait(false);
                _logger.LogInformation("End of ExecuteReaderAsync");
                var dataReader = reader.SqlDataReader;
                List<T> list = null;
                using (dataReader)
                {
                    list = dataReader.Select(mapper).ToList();
                }
                watch.Stop();
                _logger.LogInformation($"End of ExecuteReaderAsync, Total count {list?.Count}, Total Time {watch.ElapsedMilliseconds}");
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Occured In Sql Engine -ExecuteReaderAsync: " + ex.Message);
                throw ex;
            }
        }

        public SqlCommand BuildCommandWithSqlAndParameters(IDb db, string sql, List<SqlParameter> parameters, CommandOptions options = null)
        {
            var command = CreateCommand(sql, db, options, parameters);
            UpdateParametersForCommand(command, parameters);
            return command;
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities",
            Justification = "Used by system driven entries that have already been parameterized")]
        private static void UpdateParametersForCommand(SqlCommand command, List<SqlParameter> parameters)
        {          

            if (parameters == null || parameters.Count <= 0)
            {
                return;
            }

            foreach (SqlParameter p in parameters)
            {
                AddToCommand(command, p);
            }
        }

        private static void AddToCommand(SqlCommand command, SqlParameter p)
        {
            var list = p.Value as IList;
            if (list != null && !(p.Value is byte[])) //array value, we're in an "IN" statement, except for binary data
            {
                AddListToCommand(command, p, list);
            }
            else
            {
                AddParameterToCommand(command, p);
            }
        }

        private static void AddListToCommand(SqlCommand command, SqlParameter p, IEnumerable list)
        {
            var newSql = new List<string>();
            var paramList = list.Cast<object>().Distinct().ToList();
            for (var counter = 1; counter <= paramList.Count; counter++)
            {
                var name = $"{p.ParameterName}{counter}";

                AddListParameter(command, p, name, paramList[counter - 1]);
                newSql.Add(name);
            }
            command.CommandText = Regex.Replace(command.CommandText, p.ParameterName, String.Join(", ", newSql), RegexOptions.IgnoreCase);
        }

        private static void AddListParameter(SqlCommand command, SqlParameter p, string name, object item)
        {
            SetDbType(p, item);
            var parameter = new SqlParameter(name, p.SqlDbType, p.Size, p.Direction, p.IsNullable, p.Precision, p.Scale,
                p.SourceColumn, p.SourceVersion, item);
            AddParameterToCommand(command, parameter);
        }

        private static void SetDbType(SqlParameter p, object item)
        {
            try
            {
                p.SqlDbType.GetDisplayName();
            }
            catch (ArgumentException)
            {
                var temp = new SqlParameter("temp", item);
                p.SqlDbType = temp.SqlDbType;
            }
        }

        /// <summary>
        /// Helper which returns an empty string or "USE [blah];" statement
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public static string CreateEscapedUseDatabaseStatement(string database)
        {
            if (string.IsNullOrWhiteSpace(database)) return String.Empty;

            // NOTE(doug): this is not intended as a security measure, but only to correct a mistake in the create site
            // tool in that it allows beginning a StudioShort with a number. This makes no guarantee of preventing SQL
            // injection - inputs must be sanitized for a StudioShort.
            return "USE [" + database.Replace("]", "]]") + "];\n";
        }

        /// <summary>
        /// Helper which returns an empty string or "USE [blah];" statement
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private static string CreateEscapedUseDatabaseStatement(IDb db)
        {
            return CreateEscapedUseDatabaseStatement(db.UseDatabase);
        }

        private static string AddContextInfo(IDb database)
        {
            var stringBuilder = new StringBuilder("/* DataProviders");

#if DEBUG
            stringBuilder.AppendFormat(" App({0})", ConnectionStringInformation.AppName);
#endif

            // Add extra path context to provide
            if (database.Path != null)
            {
                stringBuilder.AppendFormat(" Path({0})", database.Path);
            }
            //TODO : add info about what htpp path was used to call this db
            //else if (HttpContext.Current != null)
            //{
            //    stringBuilder.AppendFormat(" Path({0})", HttpContext.Current.Request.Path);
            //}

            // Add extra into if provide (e.g. "file name, action performed, known issue")
            if (database.Info != null)
            {
                stringBuilder.AppendFormat(" Info({0})", database.Info);
            }

            // Call out DB used in the request
            if (!ConnectionStringInformation.InlineUse && database.UseDatabase != null)
            {
                stringBuilder.AppendFormat(" DB({0})", database.UseDatabase);
            }
            stringBuilder.Append(" */\n");

            return stringBuilder.ToString();
        }

        [SuppressMessage("Microsoft.Security",
            "CA2100:Review SQL queries for security vulnerabilities",
            Justification = "commandText never comes from user input and all commands are parametrized")]
        private static SqlCommand CreateCommand(string commandText, IDb db, CommandOptions options, List<SqlParameter> parameters)
        {
            var preString = string.Empty;
            if (ConnectionStringInformation.InlineComments)
            {
                preString += AddContextInfo(db);
            }
            if (ConnectionStringInformation.InlineUse)
            {
                preString += CreateEscapedUseDatabaseStatement(db);
            }

            // Drop in parameter fixing here since we need them in the T-SQL
            if (options != null && options.CommandType == CommandType.StoredProcedure)
            {
                var parameterListForStoredProcedure = CreateParameterListForStoredProcedure(parameters);
                commandText = $"exec {commandText}{parameterListForStoredProcedure}";
            }

            var command = new SqlCommand(preString + commandText, (SqlConnection)db.Connection)
            {
                Transaction = (SqlTransaction)db.Transaction
            };

#if DEBUG
            Debug.WriteLine("{0}\n{1}\n{2}",
                String.Empty.PadLeft(20, '>'),
                command.CommandText,
                String.Empty.PadLeft(20, '<'));
#endif

            if (options == null)
            {
                // Nothing to do!
                return command;
            }

            // Update with options
            if (options.CommandTimeout != null)
            {
                command.CommandTimeout = options.CommandTimeout.GetValueOrDefault();
            }

            if (options.Transaction != null)
            {
                command.Transaction = options.Transaction as SqlTransaction;
            }

            // With above T-SQL fix, this should now be text again
            command.CommandType = options.CommandType == CommandType.StoredProcedure
                ? CommandType.Text
                : options.CommandType;

            return command;
        }

        private static string CreateParameterListForStoredProcedure(List<SqlParameter> parameters)
        {
            if (parameters == null)
            {
                return string.Empty;
            }

            var paramNames = parameters.Select(p =>
            {
                var orig = p.ParameterName;
                return orig.StartsWith("@") ? orig : $"@{orig}";
            });
            return $" {String.Join(", ", paramNames)}";
        }

        private static void AddParameterToCommand(SqlCommand command, SqlParameter sqlParameter)
        {
            var key = sqlParameter.ParameterName;
            if (!key.StartsWith("@"))
            {
                key = String.Format("@{0}", key);
            }
            sqlParameter.ParameterName = key;
            sqlParameter.Value = SqlParameterFactory.GetValue(sqlParameter.Value);

            command.Parameters.Add(sqlParameter);
        }

    }
}