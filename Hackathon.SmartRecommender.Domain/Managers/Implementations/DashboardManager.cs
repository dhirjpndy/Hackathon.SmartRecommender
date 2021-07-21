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
        private readonly IAIMLReccomenderApiProvider _AIMLReccomenderApiProvider;
        private const int EmptySlotCheckInPerc = 50;

        public DashboardManager(IDashboardProvider dashboardProvider, IAIMLReccomenderApiProvider aimlReccomenderApiProvider)
        {
            _DashboardProvider = dashboardProvider;
            _AIMLReccomenderApiProvider = aimlReccomenderApiProvider;
        }

        public async Task<List<BusinessDetails>> GetBusinessesDetails()
        {
            var businessDetails =  await _DashboardProvider.GetBusinessesDetails();
            var sIds = businessDetails.Select(s => s.BusinessId).Distinct().ToList();

            var locations = await _DashboardProvider.GetLocationDetails(sIds);

            businessDetails.ForEach(b => b.LocationDetails = locations.Where(l => l.BusinessId == b.BusinessId).ToList());

            return businessDetails;
        }

        public async Task<DashboardClassDetail> GetDashboardData(int studioId, DateTime startDateTime, DateTime endDateTime)
        {
            //var response = new DashboardClassDetail();
            //var classDetails = await _DashboardProvider.GetClassDetails(studioId, startDateTime, endDateTime);

            //var totalBookedClasses = classDetails.Select(m => m.TotalVisits).Sum();
            //var totalClassCapacity = classDetails.Select(m => m.TotalCapacity).Sum();
            //var lowPerformingClasses = classDetails.Where(m => m.Score > EmptySlotCheckInPerc).Count();

            //var totalScheduledClassCount = await _DashboardProvider.GetTotalScheduledClasses(studioId, startDateTime, endDateTime);

            //response.TotalClassCapacity = totalClassCapacity;
            //response.TotalAttendees = totalBookedClasses;
            //response.TotalLowPerformingClasses = lowPerformingClasses;
            //response.TotalClasses = totalScheduledClassCount;
            //response.TotalEmptySlots = totalClassCapacity - totalBookedClasses;

            //response.TotalLossOfRevenue = -40000;
            //response.StaffUtilizaton = 55;
            //response.BusinessScore = new BusinessScore { Score = 3.4, TotalBusiness = 4 };

            //response.StaffDetails = new List<StaffDetails> { new StaffDetails { Name = "Jhon Tylor", Id = 1, StaffUtilization = 20, TotalRevenue = 100, TotalClasses = 3},
            //new StaffDetails { Name = "Ryan T", Id = 2, StaffUtilization = 40, TotalRevenue = 200, TotalClasses = 3}};

            return await Task.FromResult(PrepareDashboardDetails());
        }

        private DashboardClassDetail PrepareDashboardDetails()
        {
            return new DashboardClassDetail
            {
                TotalClasses = 80,
                TotalLowPerformingClasses = 42,
                TotalEmptySlots = 2832,
                TotalClassCapacity = 4000,
                StaffUtilizaton = 55,
                TotalLossOfRevenue = -65000,
                BusinessScore = new BusinessScore { TotalBusiness = 10, Score = 7.2 },
                StaffDetails = new List<StaffDetails>
                {
                new StaffDetails{ Name = "Alice watson", Id = 1,  StaffUtilization = 60, TotalRevenue = 1200, TotalClasses = 5},
                new StaffDetails{ Name = "Brandon Smith", Id = 2,  StaffUtilization = 58, TotalRevenue = 975, TotalClasses = 3},
                new StaffDetails{ Name = "John Wagon", Id = 3,  StaffUtilization = 42, TotalRevenue = 800, TotalClasses = 3}
                }
            };
        }

        public async Task<List<Recommendations>> GetClassRecommenders(int studioId)
        {
            //var result = await _AIMLReccomenderApiProvider.GetTimeReccommenders(studioId, "Yoga");

            //if (result != null && result.Count > 0)
            //{ 

            //}

            var result = PrepareStaticRecommendations();
            return await Task.FromResult(result);
        }

        public async Task<List<ServiceRecommendationDetails>> GetClassRecommendersInDetails(int studioId)
        {
            return await Task.FromResult(PrepareStaticRecommendationInDetailModel());
        }

        private static ServiceRecommendationDetails ToServiceMapper(string category, Time_Slot_Insights aimlDto)
        {
            return new ServiceRecommendationDetails
            {
                serviceType = category,
                Capacity = 30,
                //ServiceStartDateTime = aimlDto.Time_Slot_Start,
                //ServiceEndDateTime = aimlDto.Time_Slot_End,
                ServiceId = 12,
                NewBusinessScore = new BusinessScore { TotalBusiness = 4, Score = 3.5 },
                NewLeads = 20
            };
        }

        private static List<ServiceRecommendationDetails> PrepareStaticRecommendationInDetailModel()
        {
            var currentDate = DateTime.Now;
            return new List<ServiceRecommendationDetails>
            {
               new  ServiceRecommendationDetails
                {
                    serviceType = "Yoga",
                    ServiceStartDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 06, 00, 00).TimeOfDay,
                    ServiceEndDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 07, 00, 00).TimeOfDay,
                    Capacity = 50,
                    Day = new Days{Sunday = true, Monday = true, Tuesday = true, Wednesday = true, Thursday = true, Friday = true, Saturday = true},
                    ScheduleFor = new ScheduleFor{Number = 30, MonthYear = "days" },
                    Staff = new StaffDetails{Name = "Alice Watson", Id = 1},
                    ServiceId = 1,
                    NewBusinessScore = new BusinessScore { TotalBusiness = 10, Score = 7.5 },
                    NewLeads = 13,
                    Revenue = 1000,
                    StaffUtilization = 68,
                    StaffIncreaseInText = "Increase in 14% utilisation",
                    OffersInPercenatge = "50%",
                    Rank = 1,
                    Price = 20
               },
                new  ServiceRecommendationDetails
                {
                    serviceType = "Yoga",
                    ServiceStartDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 07, 30, 00).TimeOfDay,
                    ServiceEndDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 08, 30, 00).TimeOfDay,
                    Capacity = 40,
                    Day = new Days{Sunday = true, Saturday = true},
                    ScheduleFor = new ScheduleFor{Number = 60, MonthYear = "days" },
                    Staff = new StaffDetails{Name = "Brandon Smith", Id = 2},
                    ServiceId = 2,
                    NewBusinessScore = new BusinessScore { TotalBusiness = 10, Score = 7.4 },
                    NewLeads = 11,
                    Revenue = 800,
                    StaffUtilization = 60,
                    StaffIncreaseInText = "Increase in 10% utilisation",
                    OffersInPercenatge = "20% off on Annual Membership",
                    Rank = 2,
                    Price = 20
               },
                 new  ServiceRecommendationDetails
                {
                    serviceType = "Zumba",
                    ServiceStartDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 12, 00, 00).TimeOfDay,
                    ServiceEndDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 13, 00, 00).TimeOfDay,
                    Capacity = 25,
                    Day = new Days{Monday = true, Wednesday = true, Friday = true},
                    ScheduleFor = new ScheduleFor{Number = 90, MonthYear = "days" },
                    Staff = new StaffDetails{Name = "John Wagon", Id = 3},
                    ServiceId = 3,
                    NewBusinessScore = new BusinessScore { TotalBusiness = 10, Score = 7.3 },
                    NewLeads = 8,
                    Revenue = 750,
                    StaffUtilization = 48,
                    StaffIncreaseInText = "Increase in 8% utilisation",
                    OffersInPercenatge = "First Month Free",
                    Rank = 3,
                    Price = 30
               }
            };
        }

        private static List<Recommendations> PrepareStaticRecommendations()
        {
            var currentDate = DateTime.Now;
            return new List<Recommendations>
            {
               new  Recommendations
                {
                    serviceType = "Yoga",
                    ServiceStartDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 06, 00, 00).TimeOfDay,
                    ServiceEndDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 07, 00, 00).TimeOfDay,
                    Capacity = 50,
                    Day = new Days{Sunday = true, Monday = true, Tuesday = true, Wednesday = true, Thursday = true, Friday = true, Saturday = true},
                    ServiceId = 1,
                    Revenue = 1000,
                    Rank = 1,
                    Price = 20
               },
                new  Recommendations
                {
                    serviceType = "Yoga",
                    ServiceStartDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 07, 30, 00).TimeOfDay,
                    ServiceEndDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 08, 30, 00).TimeOfDay,
                    Capacity = 40,
                    Day = new Days{Sunday = true, Saturday = true},
                    ServiceId = 2,
                    Revenue = 800,
                    Rank = 2,
                    Price = 20
               },
                 new  Recommendations
                {
                    serviceType = "Zumba",
                    ServiceStartDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 12, 00, 00).TimeOfDay,
                    ServiceEndDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 13, 00, 00).TimeOfDay,
                    Capacity = 25,
                    Day = new Days{Monday = true, Wednesday = true, Friday = true},
                    ServiceId = 3,
                    Revenue = 750,
                    Rank = 3,
                    Price = 30
               }
            };
        }
    }
}
