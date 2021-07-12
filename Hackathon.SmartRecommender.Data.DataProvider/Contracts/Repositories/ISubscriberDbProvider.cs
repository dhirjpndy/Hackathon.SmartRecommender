using ConsumerMarketplace.AdminFlexTool.Data.SubscriberInfo;

namespace ConsumerMarketplace.AdminFlexTool.Data.Contracts.Repositories
{
    /// <summary>
    /// An interface which provides subscriber database information
    /// </summary>
    public interface ISubscriberDbProvider
    {
        /// <summary>
        /// Fetch a single subscriber's information
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <returns></returns>
        SubscriberDatabase GetSubscriberDatabaseInfo(int subscriberId);
    }
}