using CarRentalSystem.Business.DTOs.Rental;
using CarRentalSystem.Business.Helpers;
using CarRentalSystem.Business.Mappers;
using CarRentalSystem.Business.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/rentals")]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalRepository _rentalRepository;

        public RentalsController(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        [HttpGet]
        [Route("get-all-registerRental")]
        public async Task<IActionResult> GetAll([FromQuery] QueryRegisterRentalObject query)
        {
            try
            {
                var response = await _rentalRepository.GetAllAsync(query);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails { Title = "Internal Server Error", Detail = ex.Message });
            }
        }

        [HttpGet]
        [Route("single-registerRental/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await _rentalRepository.GetByIdAsync(id);
                if (response == null)
                {
                    return NotFound(new { Message = $"Rental with ID {id} not found." });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails { Title = "Internal Server Error", Detail = ex.Message });
            }
        }

        [HttpPost]
        [Route("registerRental")]
        public async Task<IActionResult> Create([FromBody] CreateRegisterRentalRequestDto rentalDto)
        {
            try
            {
                var response = await _rentalRepository.CreateAsync(rentalDto);
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response.ToRegisterRentalDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails { Title = "Internal Server Error", Detail = ex.Message });
            }
        }

        [HttpPut]
        [Route("registerRental/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRegisterRentalDto rentalDto)
        {
            try
            {
                var response = await _rentalRepository.UpdateAsync(id, rentalDto);

                if (response == null)
                {
                    return NotFound();
                }

                return Ok(response.ToRegisterRentalDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails { Title = "Internal Server Error", Detail = ex.Message });
            }
        }

        [HttpDelete]
        [Route("delete-registerRental/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _rentalRepository.DeleteAsync(id);
                if (response == null)
                {
                   return NotFound(new { Message = $"Rental with ID {id} not found." });
                }

                return NotFound(new { Message = $"Rental with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails { Title = "Internal Server Error", Detail = ex.Message });
            }  
        }
    }
}