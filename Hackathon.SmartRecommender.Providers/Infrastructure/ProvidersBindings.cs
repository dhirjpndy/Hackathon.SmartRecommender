using Hackathon.SmartRecommender.Domain.Providers;
using Hackathon.SmartRecommender.Providers.FlaskAPI;
using Hackathon.SmartRecommender.Providers.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hackathon.SmartRecommender.Providers.Infrastructure
{
    /// <summary>
    /// Bindings
    /// </summary>
    public static class ProvidersBindings
    {
        /// <summary>
        /// Registers
        /// </summary>
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            //Manager
            services.AddTransient<IDashboardProvider, DashboardProvider>();
            services.AddTransient<IAIMLReccomenderApiProvider, AIMLReccomenderApiProvider>().AddHttpClient<IAIMLReccomenderApiProvider, AIMLReccomenderApiProvider>(); ;
        }
    }
}
