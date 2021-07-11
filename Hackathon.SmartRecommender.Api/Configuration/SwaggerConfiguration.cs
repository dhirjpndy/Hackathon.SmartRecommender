using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.SmartRecommender.Api.Configuration
{
    /// <summary>
    /// Used to help drive the swashbuckle UI
    /// </summary>
    internal class SwaggerConfiguration
    {
        /// <summary>
        /// The route prefix to use
        /// </summary>
        public string? BasePath { get; set; }

        /// <summary>
        /// http or https
        /// </summary>
        public string? Scheme { get; set; }

    }
}
