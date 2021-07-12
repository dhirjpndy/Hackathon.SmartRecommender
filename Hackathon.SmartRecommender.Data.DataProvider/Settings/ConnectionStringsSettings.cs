using System.Collections.Generic;

namespace ConsumerMarketplace.AdminFlexTool.Data.Settings
{
    /// <summary>
    /// Settings for Connection Strings
    /// </summary>
    public class ConnectionStringsSettings
    {
        /// <summary>
        /// Dictionary of connection strings
        /// </summary>
        public IReadOnlyDictionary<string, string> ConnectionStrings = new Dictionary<string, string>();
    }
}
