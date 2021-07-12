using Hackathon.SmartRecommender.Domain.Managers.Contracts;
using Hackathon.SmartRecommender.Domain.Models;
using Hackathon.SmartRecommender.Domain.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.SmartRecommender.Domain.Managers.Implementations
{
    public class DashboardManager : IDashboardManager
    {
        private readonly IDashboardProvider _DashboardProvider;
        private const int EmptySlotCheckInPerc = 50;

        public DashboardManager(IDashboardProvider dashboardProvider)
        {
            _DashboardProvider = dashboardProvider;
        }

        public async Task<List<BusinessDetails>> GetBusinessesDetails()
        {
            return await _DashboardProvider.GetBusinessesDetails();
        }

        public async Task<DashboardClassDetail> GetdashboardClassData(int studioId, DateTime startDateTime, DateTime endDateTime)
        {
            var response = new DashboardClassDetail();
            var classDetails = await _DashboardProvider.GetClassDetails(studioId, startDateTime, endDateTime);

            var totalBookedClasses = classDetails.Select(m => m.TotalVisits).Sum();
            var totalClassCapacity = classDetails.Select(m => m.TotalCapacity).Sum();
            var lowPerformingClasses = classDetails.Where(m => m.Score > EmptySlotCheckInPerc).Count();

            var totalScheduledClassCount = await _DashboardProvider.GetTotalScheduledClasses(studioId, startDateTime, endDateTime);

            response.TotalClassCapacity = totalClassCapacity;
            response.TotalAttendees = totalBookedClasses;
            response.TotalLowPerformingClasses = lowPerformingClasses;
            response.TotalClasses = totalScheduledClassCount;

            return response;
        }
    }
}
