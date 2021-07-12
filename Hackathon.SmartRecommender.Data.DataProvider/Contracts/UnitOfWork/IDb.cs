using System.Data;

namespace ConsumerMarketplace.AdminFlexTool.Data.Contracts.UnitOfWork
{
    /// <summary>
    /// Interface 
    /// </summary>
    public interface IDb
    {
        /// <summary>
        /// Database to create a USE statement before execution
        /// </summary>
        string UseDatabase { get; set; }

        /// <summary>
        /// Path to list in comment of request
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// Info to list in comment of request
        /// </summary>
        string Info { get; set; }

        /// <summary>
        /// The connection that is being used
        /// </summary>
        IDbConnection Connection { get; set; }

        /// <summary>
        /// Transaction if any being used
        /// </summary>
        IDbTransaction Transaction { get; set; }
    }
}
