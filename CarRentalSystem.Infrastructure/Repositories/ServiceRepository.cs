// CarRentalSystem.Infrastructure/Repositories/ServiceRepository.cs
using CarRentalSystem.Core.Interfaces;
using CarRentalSystem.Core.Models;
using CarRentalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Repositories
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Service>> GetServicesByCarAndPeriodAsync(Guid carId, DateTime startDate, DateTime endDate)
        {
            return await _context.Services
                .Where(s => s.CarId == carId &&
                            s.ScheduledDate.Date <= endDate.Date &&
                            s.ScheduledDate.AddDays(s.DurationDays).Date >= startDate.Date &&
                            !s.IsCompleted) // Only consider active/pending services
                .ToListAsync();
        }
    }
}