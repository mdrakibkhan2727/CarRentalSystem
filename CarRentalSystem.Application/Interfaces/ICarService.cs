// CarRentalSystem.Application/Interfaces/ICarService.cs
using CarRentalSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<AvailableCarDto>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate, string? type, string? model);
        Task<CarResponseDto?> GetCarByIdAsync(Guid carId);
    }
}