using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon.SmartRecommender.Domain.Models
{
    public class ServiceRecommendationDetails
    {
        public string serviceType { get; set; }

        public int ServiceId { get; set; }

        public DateTime ServiceStartDateTime { get; set; }

        public DateTime ServiceEndDateTime { get; set; }

        public double Revenue { get; set; }

        public Days Day { get; set; }

        public double Capacity { get; set; }
        public ScheduleFor ScheduleFor { get; set; }

        public double NewLeads { get; set; }

        public BusinessScore NewBusinessScore { get; set; }

        public double StaffUtilization { get; set; }

    }

    public class ScheduleFor
    {
        public int Number { get; set; }

        public string MonthYear { get; set; }
    }

    public class Days
    {
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
    }
}
