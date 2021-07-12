using System;

namespace ConsumerMarketplace.AdminFlexTool.Data.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class SubscriberNotAvailableException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriberId"></param>
        public SubscriberNotAvailableException(int subscriberId)
            : base("Subscriber Not Available: " + subscriberId)
        {
        }
    }
}
