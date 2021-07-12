using System;

namespace ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection
{
    /// <summary>
    /// Flags which control handling of the SQL connection
    /// </summary>
    [Flags]
    public enum ConnectionFlags
    {
        /// <summary>
        /// Uses pool and read-write intent when applicable
        /// </summary>
        Default = 0x0,

        /// <summary>
        /// Override any pooling settings and disable
        /// </summary>
        DisablePooling = 0x1,

        /// <summary>
        /// Override for read-only intent
        /// </summary>
        ReadOnly = 0x2,

        /// <summary>
        /// Override for enabling Multiple Active Result Sets (MARS). Allows the execution of multiple batches on a single connection.
        /// </summary>
        EnableMars = 0x4
    }
}