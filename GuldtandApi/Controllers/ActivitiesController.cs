using Guldtand.Domain.Helpers;
using Guldtand.Domain.Models;
using Guldtand.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace GuldtandApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin,Dentist,Receptionist")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _activityService;
        private readonly ILogger<ActivitiesController> _logger;

        public ActivitiesController(
            IActivityService activityService,
            ILogger<ActivitiesController> logger
            )
        {
            _activityService = activityService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Create([FromBody] ActivityDTO activityDto)
        {
            try
            {
                _activityService.Create(activityDto);
                return Ok();
            }
            catch (AppException ex)
            {
                _logger.LogError($"Error caught in {nameof(ActivitiesController)}, details: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery(Name = "customerId")] string customerId, [FromQuery(Name = "userId")] string userId)
        {
            try
            {
                var activities = _activityService.GetAll(customerId, userId);
                return Ok(activities);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error caught in {nameof(ActivitiesController)}, details: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("future")]
        public IActionResult GetAllFuture([FromQuery(Name = "customerId")] string customerId, [FromQuery(Name = "userId")] string userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(customerId) && !string.IsNullOrEmpty(userId))
                    return BadRequest("Don't use both!");

                var activities = _activityService.GetAllFuture(customerId, userId);
                return Ok(activities);
            }
            catch (AppException ex)
            {
                _logger.LogError($"Error caught in {nameof(ActivitiesController)}, details: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var activity = _activityService.GetById(id);
            return Ok(activity);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ActivityDTO activityDto)
        {
            activityDto.Id = id;

            try
            {
                _activityService.Update(activityDto);
                return Ok();
            }
            catch (AppException ex)
            {
                _logger.LogError($"Error caught in {nameof(ActivitiesController)}, details: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _activityService.Delete(id);
            return Ok();
        }
    }
}