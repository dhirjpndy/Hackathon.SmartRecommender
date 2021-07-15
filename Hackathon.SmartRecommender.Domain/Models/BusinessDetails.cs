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
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public List<LocationDetails> LocationDetails { get; set; }
    } 

    /// <summary>
    /// location details
    /// </summary>
    public class LocationDetails
    {
        public double BusinessId { get; set; }
        public double LocationId { get; set; }

        public string LocationName { get; set; }

        public string Description { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
