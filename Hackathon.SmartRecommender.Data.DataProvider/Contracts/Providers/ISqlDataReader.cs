using System.Data;

namespace ConsumerMarketplace.AdminFlexTool.Data.Contracts.Providers
{
    /// <summary>
    /// Interface for SqlDataReader
    /// </summary>
    public interface ISqlDataReader
    {
        /// <summary>
        /// Get an IDataReader from the ISqlDataReader
        /// </summary>
        IDataReader SqlDataReader { get; set; }

        /// <summary>
        /// IsClosed
        /// </summary>
        bool IsClosed { get; }
    }
}
