using Hackathon.SmartRecommender.Domain.Managers.Contracts;
using Hackathon.SmartRecommender.Domain.Models;
using Hackathon.SmartRecommender.Domain.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.SmartRecommender.Domain.Managers.Implementations
{
    public class DashboardManager : IDashboardManager
    {
        private readonly IDashboardProvider _DashboardProvider;

        public DashboardManager(IDashboardProvider dashboardProvider)
        {
            _DashboardProvider = dashboardProvider;
        }

        public async Task<List<BusinessDetails>> GetBusinessesDetails()
        {
            return await _DashboardProvider.GetBusinessesDetails();
        }
    }
}
