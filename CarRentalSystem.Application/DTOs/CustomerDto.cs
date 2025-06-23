// CarRentalSystem.Application/DTOs/CustomerDto.cs
using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Application.DTOs
{
    public class CustomerDto
    {
        [Required]
        public string CustomerIdString { get; set; } = string.Empty;
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
    }

    public class CustomerResponseDto
    {
        public Guid Id { get; set; }
        public string CustomerIdString { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}