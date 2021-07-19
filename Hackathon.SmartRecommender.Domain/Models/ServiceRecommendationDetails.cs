using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon.SmartRecommender.Domain.Models
{
    public class Recommendations
    {
        public int Rank { get; set; }

        public double Price { get; set; }
        public string serviceType { get; set; }

        public int ServiceId { get; set; }

        public TimeSpan ServiceStartDateTime { get; set; }

        public TimeSpan ServiceEndDateTime { get; set; }

        public double Revenue { get; set; }

        public Days Day { get; set; }

        public double Capacity { get; set; }
    }
    public class ServiceRecommendationDetails : Recommendations
    {
        public ScheduleFor ScheduleFor { get; set; }

        public double NewLeads { get; set; }

        public BusinessScore NewBusinessScore { get; set; }

        public double StaffUtilization { get; set; }

        public string StaffIncreaseInText { get; set; }

        public StaffDetails Staff { get; set; }

        public string OffersInPercenatge { get; set; }
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
