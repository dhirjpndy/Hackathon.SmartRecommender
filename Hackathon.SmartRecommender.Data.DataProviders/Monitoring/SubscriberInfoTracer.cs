using System;

namespace ConsumerMarketplace.AdminFlexTool.Data.Monitoring
{
    /// <summary>
    ///     A tracer for tracking caching behavior
    /// </summary>
    public static class SubscriberInfoTracer
    {
        /// <summary>
        ///     Monitoring key for if/how Subscriber Information was retrieved
        /// </summary>
        /// <remarks>Only the first call to this key will be tracked per transaction</remarks>
        public static string CachingKey = "sqlSubInfoCache";

        /// <summary>
        ///     A value string for when subscriber information hits
        /// </summary>
        public static string CacheHitValue = "Hit";

        /// <summary>
        ///     A single listening callback for cache actions to flip dependency order
        /// </summary>
        public static Action<string, string> EventListener = null;

        /// <summary>
        ///     Private helper to call Event Listener (if set)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private static void SendEvent(string key, string value)
        {
            if (EventListener == null) return;

            try
            {
                EventListener(key, value);
            }
            catch (Exception)
            {
                // purposely empty as we want to ensure any failure in trace does affect call
            }
        }

        /// <summary>
        ///     Action performed when the miss occurred
        /// </summary>
        /// <remarks>Only the first call to this key will be tracked per transaction</remarks>
        /// <param name="infoAction"></param>
        public static void AddSubscriberInfoMiss(string infoAction)
        {
            SendEvent(CachingKey, infoAction);
        }

        /// <summary>
        ///     Let system know the subscriber information was available without DB call
        /// </summary>
        /// <remarks>Only the first call to this key will be tracked per transaction</remarks>
        public static void AddSubscriberInfoHit()
        {
            SendEvent(CachingKey, CacheHitValue);
        }
    }
}