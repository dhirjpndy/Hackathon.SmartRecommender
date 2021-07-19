using Hackathon.SmartRecommender.Domain.Models;
using Hackathon.SmartRecommender.Domain.Providers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.SmartRecommender.Providers.FlaskAPI
{
    public class AIMLReccomenderApiProvider : IAIMLReccomenderApiProvider
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AIMLReccomenderApiProvider(HttpClient httpClient, IConfiguration configuration
           )
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<AIMLReccomenderDetails>> GetTimeReccommenders(double studioId, string category)
        {
            var payload = new Dictionary<string, string>
            {
              { "Studio_ID", studioId.ToString() },
              { "Category", category }
            };

            var requestUri = $"{_configuration["ReccommenderAPI"]}/timeslotinsights";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                var errorDetail = await response.Content.ReadAsStringAsync();
                return new List<AIMLReccomenderDetails>();
            }

            var result = JsonConvert.DeserializeObject<List<AIMLReccomenderDetails>>(await response.Content.ReadAsStringAsync());
            return result;
        }
    }
}
