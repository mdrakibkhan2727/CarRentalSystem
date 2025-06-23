// CarRentalSystem.Core/Interfaces/ICarRepository.cs
using CarRentalSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalSystem.Core.Interfaces
{
    public interface ICarRepository : IRepository<Car>
    {
        Task<IEnumerable<Car>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate, string? type, string? model);
        Task<IEnumerable<Car>> GetCarsWithUpcomingServicesAsync(DateTime today, int daysAhead);
    }
}