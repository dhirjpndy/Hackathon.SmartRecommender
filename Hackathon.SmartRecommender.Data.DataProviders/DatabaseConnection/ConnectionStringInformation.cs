using ConsumerMarketplace.AdminFlexTool.Data.Settings;
using System;
using System.Diagnostics;

namespace ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection
{
    /// <summary>
    /// Static class for fetching SQL and Connection String setting information
    /// </summary>
    public class ConnectionStringInformation
    {
        private const string SqlMinPool = "SqlMinPool";
        private const string SqlMaxPool = "SqlMaxPool";


        /// <summary>
        /// The default Max Pool Size for .Net client
        /// </summary>
        public const int SqlMaxPoolSystemDefault = 100;

        /// <summary>
        /// The default Timeout for .Net client
        /// </summary>
        public const int SqlConnectionTimeoutSystemDefault = 10000;

        #region Static Configuration

        /// <summary>
        /// SQL App Username
        /// </summary>
        public static string AppUserName { get; private set; }

        /// <summary>
        /// SQL App Password
        /// </summary>
        public static string AppPassword { get; private set; }

        /// <summary>
        /// Min Pooling configuration (or neg number for disable)
        /// </summary>
        public static int MinPoolingOrNegForDisable { get; private set; }

        /// <summary>
        /// Max Pooling
        /// </summary>
        public static int MaxPoolSize { get; private set; }

        /// <summary>
        /// Multiple Active Result Sets support flag
        /// </summary>
        public static bool EnabledMars { get; private set; }

        /// <summary>
        /// Whether to include "USE" as a pre-statement or in SqlEngine
        /// </summary>
        public static bool InlineUse { get; private set; }

        /// <summary>
        /// Whether to include source and path comments in SqlEngine
        /// </summary>
        public static bool InlineComments { get; private set; }

        /// <summary>
        /// Timeout period for connections being established
        /// </summary>
        public static int ConnectionTimeout { get; private set; }

        /// <summary>
        /// SQL App Username
        /// </summary>
        public static string AppName { get; private set; }

        /// <summary>
        /// Establish a read-only intent for the request
        /// </summary>
        public static bool ReadOnly { get; private set; }

        /// <summary>
        /// Startup.cs entry point for passing in the connection strings
        /// </summary>
        /// <param name="dataProviderSettings"></param>
        public static void Init(DataProviderSettings dataProviderSettings)
        {
            AppUserName = dataProviderSettings.AppConnectionStringUser;
            if (AppUserName == null)
            {
                throw new InvalidOperationException($"Missing AppSettings key: { nameof(dataProviderSettings.AppConnectionStringUser)}");
            }
            AppPassword = dataProviderSettings.AppConnectionStringPassword;

            if (AppPassword == null)
            {
                throw new InvalidOperationException($"Missing AppSettings key: { nameof(dataProviderSettings.AppConnectionStringPassword)}");
            }

            MinPoolingOrNegForDisable = dataProviderSettings.SqlMinPool;
            MaxPoolSize = dataProviderSettings.SqlMaxPool;
            if (MaxPoolSize == 0)
            {
                MaxPoolSize = SqlMaxPoolSystemDefault;
            }

            if (MaxPoolSize < MinPoolingOrNegForDisable)
            {
                throw new InvalidOperationException(SqlMaxPool + " must be greater than " + SqlMinPool);
            }

            EnabledMars = dataProviderSettings.SqlEnableMars;
            InlineUse = dataProviderSettings.SqlInlineUse;
            InlineComments = dataProviderSettings.SqlInlineComments;

            // Set application name via app.config or default to assembly name
            AppName = dataProviderSettings.SqlAppName ??
                      Process.GetCurrentProcess().ProcessName;

            ConnectionTimeout = dataProviderSettings.SqlConnectionTimeout;
            if (ConnectionTimeout == 0)
            {
                ConnectionTimeout = SqlConnectionTimeoutSystemDefault;
            }
        }

        #endregion
    }
}
