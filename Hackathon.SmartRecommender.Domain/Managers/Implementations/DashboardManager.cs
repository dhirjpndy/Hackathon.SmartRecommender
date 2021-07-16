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
            var businessDetails =  await _DashboardProvider.GetBusinessesDetails();
            var sIds = businessDetails.Select(s => s.BusinessId).Distinct().ToList();

            var locations = await _DashboardProvider.GetLocationDetails(sIds);

            businessDetails.ForEach(b => b.LocationDetails = locations.Where(l => l.BusinessId == b.BusinessId).ToList());

            return businessDetails;
        }

        public async Task<DashboardClassDetail> GetDashboardClassData(int studioId, DateTime startDateTime, DateTime endDateTime)
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
            response.TotalEmptySlots = totalClassCapacity - totalBookedClasses;

            response.TotalLossOfRevenue = -40000;
            response.StaffUtilizaton = 55;
            response.BusinessScore = new BusinessScore { Score = 3.4, TotalBusiness = 4 };

            response.StaffDetails = new List<StaffDetails> { new StaffDetails { Name = "Jhon Tylor", Id = 1, StaffUtilization = 20, TotalRevenue = 100, TotalClasses = 3},
            new StaffDetails { Name = "Ryan T", Id = 2, StaffUtilization = 40, TotalRevenue = 200, TotalClasses = 3}};

            return response;
        }

        public async Task<List<ServiceRecommendationDetails>> GetClassRecommenders(int studioId, DateTime startDateTime, DateTime endDateTime)
        {
            return await Task.FromResult(new List<ServiceRecommendationDetails>());
        }
    }
}
