// CarRentalSystem.Core/Interfaces/IServiceRepository.cs
using CarRentalSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalSystem.Core.Interfaces
{
    public interface IServiceRepository : IRepository<Service>
    {
        Task<IEnumerable<Service>> GetServicesByCarAndPeriodAsync(Guid carId, DateTime startDate, DateTime endDate);
    }
}