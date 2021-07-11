using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Hackathon.SmartRecommender.Api.Infrastructure
{
    /// <summary>
    /// Bindings
    /// </summary>
    public static class ApiBindings
    {
        /// <summary>
        /// Registers
        /// </summary>
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IPrincipal>(x => x.GetService<IHttpContextAccessor>().HttpContext.User);
        }
    }
}
