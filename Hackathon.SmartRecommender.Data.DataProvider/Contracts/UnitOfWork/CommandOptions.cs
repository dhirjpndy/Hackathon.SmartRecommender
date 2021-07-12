using System.Data;

namespace ConsumerMarketplace.AdminFlexTool.Data.Contracts.UnitOfWork
{
    /// <summary>
    /// Set of command options
    /// </summary>
    public class CommandOptions
    {
        /// <summary>
        /// Wait time before terminating command in seconds
        /// </summary>
        public int? CommandTimeout { get; set; }

        /// <summary>
        /// How the command is to be interpreted
        /// </summary>
        public CommandType CommandType { get; set; }

        /// <summary>
        /// Sql transaction to be executed
        /// </summary>
        public IDbTransaction Transaction { get; set; }
    }
}
