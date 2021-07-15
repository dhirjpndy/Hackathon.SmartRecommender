using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon.SmartRecommender.Domain.Models
{
    public class ClassSchedulingDetails
    {
        public double DescriptionId { get; set; }
        public double Score { get; set; }
        public double StudioID { get; set; }
        public double TotalVisits { get; set; }
        public double TotalCapacity { get; set; }
    }

    public class DashboardClassDetail
    {
        public int TotalClasses { get; set; }
        public int TotalLowPerformingClasses { get; set; }
        public double TotalClassCapacity { get; set; }

        public double TotalAttendees { get; set; }

        public double TotalEmptySlots { get; set; }

        public float StaffUtilizaton { get; set; }

        //static
        public BusinessScore BusinessScore { get; set; }

        public double TotalLossOfRevenue { get; set; }

        public List<StaffDetails> StaffDetails { get; set; }
    }

    public class BusinessScore
    {
        //static
        public double Score { get; set; }

        //static
        public double TotalBusiness { get; set; }
    }

    public class StaffDetails
    {
        //static
        public string Name { get; set; }

        //static
        public int Id { get; set; }

        public double TotalRevenue { get; set; }

        public double StaffUtilization { get; set; }

        public double TotalClasses { get; set; }
    }
}
