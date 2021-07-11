using Hackathon.SmartRecommender.Api.Configuration;
using Hackathon.SmartRecommender.Api.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.SmartRecommender.Api.Extensions
{
    /// <summary>
    /// Extension of Application Builder 
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Registers the health ping endpoints used by Kubernetes
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder AddKubernetesHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health/live", new HealthCheckOptions()
            {
                // Exclude all checks and return a 200-Ok.
                Predicate = (_) => false
            });

            app.UseHealthChecks("/health/ready", new HealthCheckOptions()
            {
                // Exclude all checks and return a 200-Ok.
                Predicate = (_) => false
            });

            return app;
        }

        /// <summary>
        /// Configures Swagger to generate the API documentation
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder SetupSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            var swaggerConfiguration = configuration.GetSection("Swagger").Get<SwaggerConfiguration>();

            app.UseSwagger(c =>
            {
                // from https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/427
                // this is needed to work from behind a proxy
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{swaggerConfiguration.Scheme}://{httpReq.Host.Value}{swaggerConfiguration.BasePath}" } };
                });
            });

            app.UseSwaggerUI(o =>
            {
                // from https://github.com/domaindrivendev/Swashbuckle.AspNetCore#change-the-path-for-swagger-json-endpoints
                // the default path for all documents is" "/swagger/{docummentName}/swagger.json"
                o.SwaggerEndpoint($"{swaggerConfiguration.BasePath}/swagger/{ApiConstants.ApiVersion}/swagger.json", ApiConstants.ApiVersion);
                o.OAuthAppName(ApiConstants.ApiTitle);
                o.RoutePrefix = ""; 
            });

            return app;
        }

        /// <summary>
        /// Ensures all endpoints are secured
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder RequireAuthorizationForAllEndpoints(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                var conventionBuilder = endpoints.MapDefaultControllerRoute();

                conventionBuilder.RequireAuthorization();
            });

            return app;
        }
    }
}
