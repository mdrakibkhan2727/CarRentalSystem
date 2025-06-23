// CarRentalSystem.Api/Controllers/RentalsController.cs
using CarRentalSystem.Application.DTOs;
using CarRentalSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CarRentalSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        /// <summary>
        /// Registers a new car rental.
        /// </summary>
        /// <param name="rentalDto">Details for the new rental.</param>
        /// <returns>A newly created rental response.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(RentalResponseDto), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)] // Customer or Car not found
        [ProducesResponseType(typeof(ProblemDetails), 409)] // Conflict (e.g., car not available)
        public async Task<IActionResult> RegisterRental([FromBody] RegisterRentalDto rentalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _rentalService.RegisterRentalAsync(rentalDto);
                return CreatedAtAction(nameof(GetRentalById), new { rentalId = response.RentalId }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ProblemDetails { Title = "Invalid Request", Detail = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // This covers car not found, customer not found, and car not available
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(new ProblemDetails { Title = "Resource Not Found", Detail = ex.Message });
                }
                return Conflict(new ProblemDetails { Title = "Rental Conflict", Detail = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails { Title = "Internal Server Error", Detail = ex.Message });
            }
        }

        /// <summary>
        /// Modifies an existing rental reservation.
        /// </summary>
        /// <param name="rentalId">The unique ID of the rental to modify.</param>
        /// <param name="updateDto">New details for the rental.</param>
        /// <returns>The updated rental response.</returns>
        [HttpPut("{rentalId:guid}")]
        [ProducesResponseType(typeof(RentalResponseDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(404)] // Rental or new Car not found
        [ProducesResponseType(typeof(ProblemDetails), 409)] // Conflict (e.g., car not available with new dates/car)
        public async Task<IActionResult> ModifyReservation(Guid rentalId, [FromBody] UpdateRentalDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _rentalService.ModifyReservationAsync(rentalId, updateDto);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ProblemDetails { Title = "Invalid Request", Detail = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(new ProblemDetails { Title = "Resource Not Found", Detail = ex.Message });
                }
                return Conflict(new ProblemDetails { Title = "Reservation Conflict", Detail = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails { Title = "Internal Server Error", Detail = ex.Message });
            }
        }

        /// <summary>
        /// Cancels a previously registered rental.
        /// </summary>
        /// <param name="rentalId">The unique ID of the rental to cancel.</param>
        /// <returns>No content on successful cancellation.</returns>
        [HttpDelete("{rentalId:guid}")]
        [ProducesResponseType(204)] // No Content
        [ProducesResponseType(404)] // Rental not found
        [ProducesResponseType(typeof(ProblemDetails), 400)] // Cannot cancel if already started
        public async Task<IActionResult> CancelRental(Guid rentalId)
        {
            try
            {
                await _rentalService.CancelRentalAsync(rentalId);
                return NoContent(); // 204 No Content for successful deletion
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return NotFound();
                }
                return BadRequest(new ProblemDetails { Title = "Cancellation Failed", Detail = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails { Title = "Internal Server Error", Detail = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves details of a specific rental.
        /// </summary>
        /// <param name="rentalId">The unique ID of the rental.</param>
        /// <returns>Rental details.</returns>
        [HttpGet("{rentalId:guid}")]
        [ProducesResponseType(typeof(RentalResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRentalById(Guid rentalId)
        {
            var rental = await _rentalService.GetRentalByIdAsync(rentalId);
            if (rental == null)
            {
                return NotFound();
            }
            return Ok(rental);
        }
    }
}