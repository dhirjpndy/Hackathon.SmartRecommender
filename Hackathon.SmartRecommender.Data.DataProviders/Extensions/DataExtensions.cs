using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ConsumerMarketplace.AdminFlexTool.Reports.Data.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataExtensions
    {
        /// <summary>
        /// Selects the specified projection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="projection">The projection.</param>
        /// <returns></returns>
        public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataReader, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static T GetValue<T>(this IDataReader reader, string columnName) =>  reader[columnName] is DBNull ? default : (T)reader[columnName];

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static T GetValue<T>(this object obj) => obj is DBNull ? default : (T)obj;

        /// <summary>
        /// Gets name for the enum from the name property of the display attribute.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum e)
        {
            var name = e.ToString();
            var memberInfo = e.GetType().GetMember(name).FirstOrDefault();

            if (memberInfo != null)
            {
                var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    return displayAttribute.Name;
                }
            }

            return name;
        }
    }
}







