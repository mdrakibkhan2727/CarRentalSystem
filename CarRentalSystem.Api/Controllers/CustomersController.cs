// CarRentalSystem.Api/Controllers/CustomersController.cs
using CarRentalSystem.Application.DTOs;
using CarRentalSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CarRentalSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Registers a new customer in the system.
        /// </summary>
        /// <param name="customerDto">Customer details for registration.</param>
        /// <returns>A newly created customer response.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerResponseDto), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 409)] // Conflict for duplicate ID
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _customerService.RegisterCustomerAsync(customerDto);
                // Return 201 Created with the location of the new resource
                return CreatedAtAction(nameof(GetCustomerByIdString), new { customerIdString = response.CustomerIdString }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ProblemDetails { Title = "Registration Conflict", Detail = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ProblemDetails { Title = "Invalid Input", Detail = ex.Message });
            }
            catch (Exception ex)
            {
                // General catch-all for unexpected errors
                return StatusCode(500, new ProblemDetails { Title = "Internal Server Error", Detail = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves details of a specific customer by their string ID.
        /// </summary>
        /// <param name="customerIdString">The unique string ID of the customer.</param>
        /// <returns>Customer details.</returns>
        [HttpGet("{customerIdString}")]
        [ProducesResponseType(typeof(CustomerResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCustomerByIdString(string customerIdString)
        {
            var customer = await _customerService.GetCustomerByIdStringAsync(customerIdString);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }
    }
}