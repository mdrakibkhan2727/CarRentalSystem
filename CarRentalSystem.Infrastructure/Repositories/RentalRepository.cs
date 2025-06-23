// CarRentalSystem.Infrastructure/Repositories/RentalRepository.cs
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
    public class RentalRepository : Repository<Rental>, IRentalRepository
    {
        public RentalRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Rental>> GetRentalsByCarAndPeriodAsync(Guid carId, DateTime startDate, DateTime endDate)
        {
            return await _context.Rentals
                .Where(r => r.CarId == carId &&
                            ((r.StartDate.Date < endDate.Date && r.EndDate.Date > startDate.Date) || // Overlapping rentals
                             r.EndDate.Date == startDate.AddDays(-1).Date)) // Next-day rule check
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetAllRentalsInDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Rentals
                .Where(r => r.StartDate.Date >= startDate.Date && r.EndDate.Date <= endDate.Date)
                .ToListAsync();
        }

        //public override async Task<Rental?> GetByIdAsync(Guid id)
        //{
        //    return await _context.Rentals
        //        .Include(r => r.Customer) // Include customer for response DTO
        //        .Include(r => r.Car)       // Include car for response DTO
        //        .FirstOrDefaultAsync(r => r.Id == id);
        //}
    }
}