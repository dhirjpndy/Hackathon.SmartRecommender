using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.SmartRecommender.Domain.Managers.Contracts;
using Hackathon.SmartRecommender.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.SmartRecommender.Api.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("v{version:apiVersion}/Recommender")]
    public class RecommenderController : ControllerBase
    {
        private readonly IDashboardManager _DashboardManager;

        public RecommenderController(IDashboardManager dashboardManager)
        {
            _DashboardManager = dashboardManager;
        }

        /// <summary>
        /// Get recommendations
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Studios/{studioId}")]
        [ProducesResponseType(typeof(List<Recommendations>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Recommendations>>> GetClassRecommender(int studioId)
        {
            var result = await _DashboardManager.GetClassRecommenders(studioId);
            return Ok(result);
        }

        /// <summary>
        /// Get Recommnedations in details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Studios/{studioId}/Configuration")]
        [ProducesResponseType(typeof(List<ServiceRecommendationDetails>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ServiceRecommendationDetails>>> GetClassRecommenderInDetails(int studioId)
        {
            var result = await _DashboardManager.GetClassRecommendersInDetails(studioId);
            return Ok(result);
        }
    }
}
