using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.SmartRecommender.Api.Configuration
{
    /// <summary>
    /// CORS Configurations
    /// </summary>
    public class CorsConfigurations
    {
        /// <summary>
        /// Allowed origins
        /// </summary>
        public List<string> AllowedOrigins { get; set; } = new List<string>();
    }
}
