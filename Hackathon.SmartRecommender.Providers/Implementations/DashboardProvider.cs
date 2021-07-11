using Hackathon.SmartRecommender.Domain.Models;
using Hackathon.SmartRecommender.Domain.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.SmartRecommender.Providers.Implementations
{
    public class DashboardProvider : IDashboardProvider
    {
        public async Task<List<BusinessDetails>> GetBusinessesDetails()
        {
            return await Task.FromResult(new List<BusinessDetails>());
        }
    }
}
