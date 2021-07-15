using Hackathon.SmartRecommender.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.SmartRecommender.Domain.Providers
{
    public interface IDashboardProvider
    {
        Task<List<BusinessDetails>> GetBusinessesDetails();

        Task<List<LocationDetails>> GetLocationDetails(List<double> StudioIds);

        Task<List<ClassSchedulingDetails>> GetClassDetails(int studioId, DateTime startDateTime, DateTime endDateTime);

        Task<int> GetTotalScheduledClasses(int studioId, DateTime? startDateTime, DateTime? endDateTime);
    }
}
