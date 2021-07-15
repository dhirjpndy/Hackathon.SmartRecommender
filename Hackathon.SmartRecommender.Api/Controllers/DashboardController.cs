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
        [Route("Studios/{studioId}/ClassDetail")]
        [ProducesResponseType(typeof(List<DashboardClassDetail>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DashboardClassDetail>>> GetDashboardClassDetails(int studioId, DateTime? startDateTime, DateTime? endDateTime)
        {
            if (!startDateTime.HasValue)
                startDateTime = new DateTime(2019, 01, 01);

            if (!endDateTime.HasValue)
                endDateTime = new DateTime(2019, 12, 31);

            var result = await _DashboardManager.GetDashboardClassData(studioId, startDateTime.Value, endDateTime.Value);
            return Ok(result);
        }
    }
}
