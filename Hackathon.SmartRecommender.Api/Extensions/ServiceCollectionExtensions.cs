using Hackathon.SmartRecommender.Api.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.SmartRecommender.Api.Extensions
{
    /// <summary>
    /// Service Collection Extension
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Enables CORS support and uses appsettings to configure
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection EnableCors(this IServiceCollection services, IConfiguration configuration)
        {
            var corsConfigurations = configuration.GetSection("Cors").Get<CorsConfigurations>();
            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", corsBuilder =>
                {
                    corsBuilder.AllowAnyHeader();
                    corsBuilder.AllowAnyMethod();
                    corsBuilder.AllowCredentials();
                    corsBuilder.SetIsOriginAllowedToAllowWildcardSubdomains();
                    corsBuilder.WithOrigins(corsConfigurations.AllowedOrigins.ToArray());
                });
            });

            return services;
        }
        /// <summary>
        ///  Configures Swagger to generate the API documentation
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(o =>
            {
                var swaggerFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!,
                    "Hackathon.SmartRecommender.Api.xml");

                if (File.Exists(swaggerFilePath))
                {
                    o.IncludeXmlComments(swaggerFilePath);
                }
            });
            return services;
        }

    }
}
