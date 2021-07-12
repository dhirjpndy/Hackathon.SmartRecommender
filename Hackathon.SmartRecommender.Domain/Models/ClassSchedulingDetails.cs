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

        public float StaffUtilizaton { get; set; }
    }
}
