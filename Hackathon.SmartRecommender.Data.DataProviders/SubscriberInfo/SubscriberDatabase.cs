using System.Runtime.Serialization;

namespace ConsumerMarketplace.AdminFlexTool.Data.SubscriberInfo
{
    /// <summary>
    ///     A container for reference data needed to connect to a subscriber DB
    /// </summary>
    [DataContract]
    public class SubscriberDatabase
    {
        /// <summary>
        ///     Client's subscriber Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     SQL Server the client's database lives on
        /// </summary>
        [DataMember]
        public string ServerName { get; set; }

        /// <summary>
        ///     SQL DB (on <see cref="ServerName" />) which is the client's
        /// </summary>
        [DataMember]
        public string DatabaseName { get; set; }
    }
}