using ConsumerMarketplace.AdminFlexTool.Data.Contracts.UnitOfWork;
using System.Data;

namespace ConsumerMarketplace.AdminFlexTool.Data.UnitOfWork
{
    /// <summary>
    /// Represents a database connection
    /// </summary>
    public class Db : IDb
    {
        /// <summary>
        /// Database to create a USE statement before execution
        /// </summary>
        public string UseDatabase { get; set; }

        /// <summary>
        /// Path to list in comment of request
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Info to list in comment of request
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// The connection that is being used
        /// </summary>
        public IDbConnection Connection { get; set; }

        /// <summary>
        /// Transaction if any being used
        /// </summary>
        public IDbTransaction Transaction { get; set; }
    }
}