// CarRentalSystem.Application/DTOs/RentalDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Application.DTOs
{
    public class RegisterRentalDto
    {
        [Required]
        public string CustomerIdString { get; set; } = string.Empty;
        [Required]
        public Guid CarId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }

    public class UpdateRentalDto
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public Guid? CarId { get; set; } // Optional if only dates are changing
    }

    public class RentalResponseDto
    {
        public Guid RentalId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerFullName { get; set; } = string.Empty;
        public Guid CarId { get; set; }
        public string CarType { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}