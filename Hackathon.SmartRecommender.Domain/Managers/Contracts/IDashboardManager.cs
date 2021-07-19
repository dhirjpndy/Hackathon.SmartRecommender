using Hackathon.SmartRecommender.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.SmartRecommender.Domain.Managers.Contracts
{
    public interface IDashboardManager
    {
        Task<List<BusinessDetails>> GetBusinessesDetails();
        Task<DashboardClassDetail> GetDashboardData(int studioId, DateTime startDateTime, DateTime endDateTime);

        Task<List<Recommendations>> GetClassRecommenders(int studioId);

        Task<List<ServiceRecommendationDetails>> GetClassRecommendersInDetails(int studioId);
    }
}
