// CarRentalSystem.Api/Controllers/CarsController.cs
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
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        /// <summary>
        /// Checks the availability of cars based on date range and optional features.
        /// </summary>
        /// <param name="startDate">Start date of the rental (YYYY-MM-DD).</param>
        /// <param name="endDate">End date of the rental (YYYY-MM-DD).</param>
        /// <param name="type">Optional: Car type (e.g., "SUV", "Sedan").</param>
        /// <param name="model">Optional: Car model (e.g., "CR-V", "Civic").</param>
        /// <returns>A list of available cars.</returns>
        [HttpGet("available")]
        [ProducesResponseType(typeof(IEnumerable<AvailableCarDto>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> GetAvailableCars(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string? type = null,
            [FromQuery] string? model = null)
        {
            if (startDate > endDate)
            {
                return BadRequest(new ProblemDetails { Title = "Invalid Date Range", Detail = "Start date cannot be after end date." });
            }

            try
            {
                var availableCars = await _carService.GetAvailableCarsAsync(startDate.Date, endDate.Date, type, model);
                return Ok(availableCars);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails { Title = "Internal Server Error", Detail = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves detailed information about a specific car.
        /// </summary>
        /// <param name="carId">The unique ID of the car.</param>
        /// <returns>Car details.</returns>
        [HttpGet("{carId:guid}")]
        [ProducesResponseType(typeof(CarResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCarById(Guid carId)
        {
            var car = await _carService.GetCarByIdAsync(carId);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }
    }
}