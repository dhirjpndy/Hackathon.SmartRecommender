using Hackathon.SmartRecommender.Domain.Managers.Contracts;
using Hackathon.SmartRecommender.Domain.Managers.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon.SmartRecommender.Domain.Infrastructure
{
    /// <summary>
    /// Bindings
    /// </summary>
    public static class DomainBindings
    {
        /// <summary>
        /// Registers
        /// </summary>
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            //Manager
            services.AddTransient<IDashboardManager, DashboardManager>();
        }
    }
}
