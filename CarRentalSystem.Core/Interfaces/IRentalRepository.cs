// CarRentalSystem.Core/Interfaces/IRentalRepository.cs
using CarRentalSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalSystem.Core.Interfaces
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Task<IEnumerable<Rental>> GetRentalsByCarAndPeriodAsync(Guid carId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Rental>> GetAllRentalsInDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}