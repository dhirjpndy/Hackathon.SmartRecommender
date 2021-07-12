using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon.SmartRecommender.Domain.Models
{
    /// <summary>
    /// business details
    /// </summary>
    public class BusinessDetails
    {
        public double BusinessId { get; set; }

        public string BusinessName { get; set; }

        public string BusinessDescription { get; set; }
    }
}
