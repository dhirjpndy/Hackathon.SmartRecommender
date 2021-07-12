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
    [Route("v{version:apiVersion}/Dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardManager _DashboardManager;

        public DashboardController(IDashboardManager dashboardManager)
        {
            _DashboardManager = dashboardManager;
        }

        /// <summary>
        /// Get Studios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Studios")]
        [ProducesResponseType(typeof(List<BusinessDetails>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BusinessDetails>>> GetStudios()
        {
            var result = await _DashboardManager.GetBusinessesDetails();
            return Ok(result);
        }

        /// <summary>
        /// Get Studios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Studios/{studioId}")]
        [ProducesResponseType(typeof(List<BusinessDetails>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BusinessDetails>>> GetDashboardClassDetails(int studioId, DateTime startDateTime, DateTime endDateTime)
        {
            var result = await _DashboardManager.GetdashboardClassData(studioId, startDateTime, endDateTime);
            return Ok(result);
        }
    }
}
