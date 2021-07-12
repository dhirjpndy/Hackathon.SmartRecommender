namespace ConsumerMarketplace.AdminFlexTool.Data.Contracts.UnitOfWork
{
    /// <summary>
    /// Represents a db context
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// The db of this context
        /// </summary>
        IDb Db { get; set; }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        string ConnectionString { get; }

        /// <summary>
        /// Creates and stores the db for this context
        /// </summary>
        /// <returns></returns>
        IDb CreateAndStore();
    }
}