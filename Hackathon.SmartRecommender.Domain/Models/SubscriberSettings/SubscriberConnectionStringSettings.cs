using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon.SmartRecommender.Domain.Models.SubscriberSettings
{
    public class SubscriberConnectionStringSettings
    {
        /// <summary>
        /// subscriber connection string value
        /// </summary>
        public string SubscriberConnectionStringTemplate { get; set; } = string.Empty;

        /// <summary>
        /// TimeOut value for SQL Command
        /// </summary>
        public string SqlCommandTimeOut { get; set; } = string.Empty;
    }
}
