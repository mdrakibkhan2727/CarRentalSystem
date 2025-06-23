// CarRentalSystem.Application/DTOs/CarDto.cs
using System;

namespace CarRentalSystem.Application.DTOs
{
    public class CarResponseDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public DateTime LastServiceDate { get; set; }
    }

    public class AvailableCarDto
    {
        public Guid CarId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public DateTime LastServiceDate { get; set; }
    }
}