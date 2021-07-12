using System;
using System.Data;
using System.Data.SqlClient;

namespace ConsumerMarketplace.AdminFlexTool.Data.Factories
{
    /// <summary>
    /// Provides common factory methods for <see cref="SqlParameter"/>
    /// </summary>
    public static class SqlParameterFactory
    {
        /// <summary>
        /// Creates parameter with name, sqlDbType, and value.  Will assign <see cref="DBNull.Value"/> if <paramref name="value"/> is null
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sqlDbType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static SqlParameter Create(string name, SqlDbType sqlDbType, object value)
        {
            // Optimization for SQL when variable size is auto-set to value length. Stack Overflow Article:
            // https://stackoverflow.com/questions/14342954/ado-net-safe-to-specify-1-for-sqlparameter-size-for-all-varchar-parameters
            if (sqlDbType != SqlDbType.VarChar && sqlDbType != SqlDbType.NVarChar)
            {
                return new SqlParameter(name, sqlDbType)
                {
                    Value = value ?? DBNull.Value
                };
            }

            var size = sqlDbType == SqlDbType.VarChar ? 8000 : 4000;

            if (value != null && !(value is DBNull)
                && value.ToString().Length > size)
            {
                size = -1;
            }

            return Create(name, sqlDbType, size, value);
        }

        /// <summary>
        /// Creates parameter with name, sqlDbType, size, and value.  Will assign <see cref="DBNull.Value"/> if <paramref name="value"/> is null
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sqlDbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static SqlParameter Create(string name, SqlDbType sqlDbType, int size, object value)
        {
            return new SqlParameter(name, sqlDbType, size)
            {
                Value = value ?? DBNull.Value
            };
        }

        /// <summary>
        ///     Helper method to convert any incoming object into an object that sqlCommand understands
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal static object GetValue(object item)
        {
            if (item == null)
            {
                return DBNull.Value;
            }

            var t = item.GetType();

            // Convert enum to an integer
            if (t.IsEnum)
            {
                return (int)item;
            }

            return item;
        }
    }
}