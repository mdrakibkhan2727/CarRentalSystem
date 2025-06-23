// CarRentalSystem.Api/Controllers/ReportsController.cs
using CarRentalSystem.Application.DTOs;
using CarRentalSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Generates a list of cars with scheduled services in the next two weeks.
        /// </summary>
        /// <param name="daysAhead">Number of days into the future to check for services (default: 14).</param>
        /// <returns>A list of upcoming services.</returns>
        [HttpGet("upcoming-services")]
        [ProducesResponseType(typeof(IEnumerable<UpcomingServiceDto>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetUpcomingServices([FromQuery] int daysAhead = 14)
        {
            try
            {
                // Using DateTime.Today to ensure consistent "today" for the report, ignoring time.
                var upcomingServices = await _reportService.GetUpcomingServicesAsync(DateTime.Today, daysAhead);
                return Ok(upcomingServices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails { Title = "Internal Server Error", Detail = ex.Message });
            }
        }

        /// <summary>
        /// Provides statistics on the most rented car type and its utilization percentage within a date range.
        /// </summary>
        /// <param name="startDate">Start date for the statistics calculation (YYYY-MM-DD).</param>
        /// <param name="endDate">End date for the statistics calculation (YYYY-MM-DD).</param>
        /// <returns>Car utilization statistics.</returns>
        [HttpGet("car-utilization")]
        [ProducesResponseType(typeof(CarUtilizationReportDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetCarUtilizationStatistics(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                // Ensure only date part is used for comparison
                var report = await _reportService.GetCarUtilizationStatisticsAsync(startDate.Date, endDate.Date);
                return Ok(report);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ProblemDetails { Title = "Invalid Date Range", Detail = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails { Title = "Internal Server Error", Detail = ex.Message });
            }
        }
    }
}