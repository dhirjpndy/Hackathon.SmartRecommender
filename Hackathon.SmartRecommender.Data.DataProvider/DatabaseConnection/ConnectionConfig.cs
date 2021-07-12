namespace ConsumerMarketplace.AdminFlexTool.Data.DatabaseConnection
{
    /// <summary>
    /// A parameter for data provider creation which scopes intent for the use
    /// </summary>
    public class ConnectionConfig
    {
        /// <summary>
        /// Flags for special handling of the connection
        /// </summary>
        public ConnectionFlags Flags { get; set; }

        /// <summary>
        /// (Optional) Username to override default
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// (Optional) Password to override default
        /// </summary>
        public string Password { get; set; }

        #region Flag Helpers

        /// <summary>
        /// Establish a read-only intent for the request for availability group use
        /// </summary>
        public bool ReadOnly => Flags.HasFlag(ConnectionFlags.ReadOnly);

        /// <summary>
        /// Disable pooling for connection no matter global configuration value
        /// </summary>
        public bool DisablePooling => Flags.HasFlag(ConnectionFlags.DisablePooling);


        /// <summary>
        /// Enable Multiple Active Result Sets for connection no matter global configuration value
        /// </summary>
        public bool EnableMars => Flags.HasFlag(ConnectionFlags.EnableMars);


        /// <summary>
        /// Implicit type conversion for flags for easier use and simplier contracts
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static implicit operator ConnectionConfig(ConnectionFlags flags)
        {
            return new ConnectionConfig
            {
                Flags = flags
            };
        }

        #endregion
    }
}