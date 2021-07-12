using ConsumerMarketplace.AdminFlexTool.Data.Contracts.UnitOfWork;
using System.Data;

namespace ConsumerMarketplace.AdminFlexTool.Data.Factories
{
    /// <summary>
    /// Helper objects and constants to reduce object creation and cleanup
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// CommandOption without any settings
        /// </summary>
        public static readonly CommandOptions CommandOptionDefault = new CommandOptions
        {
            CommandType = CommandType.StoredProcedure
        };

        /// <summary>
        /// CommandOption with Stored Procedure Set
        /// </summary>
        public static readonly CommandOptions CommandOptionStoredProcedure = new CommandOptions
        {
            CommandType = CommandType.StoredProcedure
        };

        /// <summary>
        /// Helper method to migrate a common pattern of using one of the above objects
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <returns></returns>
        internal static CommandOptions CommandOptionForStoredProcFlag(bool storedProcedure)
        {
            return storedProcedure ? CommandOptionStoredProcedure : null;
        }
    }
}
