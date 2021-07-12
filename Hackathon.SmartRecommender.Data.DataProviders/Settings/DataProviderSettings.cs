namespace ConsumerMarketplace.AdminFlexTool.Data.Settings
{
    /// <summary>
    /// DataProvider Settings
    /// </summary>
    public class DataProviderSettings
    {
        /// <summary>
        /// SqlMinPool
        /// </summary>
        public int SqlMinPool { get; set; }

        /// <summary>
        /// MaxPool
        /// </summary>
        public int SqlMaxPool { get; set; }

        /// <summary>
        /// Mar Enabled
        /// </summary>
        public bool SqlEnableMars { get; set; }

        /// <summary>
        /// InLine use
        /// </summary>
        public bool SqlInlineUse { get; set; }

        /// <summary>
        /// Inline comments allowed
        /// </summary>
        public bool SqlInlineComments { get; set; }

        /// <summary>
        /// Connection Timeout
        /// </summary>
        public int SqlConnectionTimeout { get; set; }

        /// <summary>
        /// Cache Stratagy
        /// </summary>
        public string SubProviderCacheStrategy { get; set; }

        /// <summary>
        /// App Name
        /// </summary>
        public string SqlAppName { get; set; }

        /// <summary>
        /// Sql User
        /// </summary>
        public string AppConnectionStringUser { get; set; }

        /// <summary>
        /// Sql pwd
        /// </summary>
        public string AppConnectionStringPassword { get; set; }
    }
}
