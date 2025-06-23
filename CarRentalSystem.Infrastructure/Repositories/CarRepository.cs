// CarRentalSystem.Infrastructure/Repositories/CarRepository.cs
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
    public class CarRepository : Repository<Car>, ICarRepository
    {
        public CarRepository(ApplicationDbContext context) : base(context)
        {
        }

        // This method's logic will mostly be handled by the CarService using IsAvailable on the Car model,
        // but we can pre-filter here.
        public async Task<IEnumerable<Car>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate, string? type, string? model)
        {
            var query = _context.Cars
                .Include(c => c.Rentals) // Include rentals to check availability
                .Include(c => c.Services) // Include services to check availability
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(c => c.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(model))
            {
                query = query.Where(c => c.Model.Equals(model, StringComparison.OrdinalIgnoreCase));
            }

            // We fetch all potential cars and let the domain logic (Car.IsAvailable) filter
            // This is a simplified approach, for very large datasets, this query might need optimization.
            var cars = await query.ToListAsync();

            // The actual availability check is delegated to the domain model in CarService
            return cars;
        }

        public async Task<IEnumerable<Car>> GetCarsWithUpcomingServicesAsync(DateTime today, int daysAhead)
        {
            var futureDate = today.AddDays(daysAhead);

            // Find cars that have a service scheduled within the next 'daysAhead' from today
            // Or if they don't have a specific service record, predict based on lastServiceDate
            var carsWithServices = await _context.Cars
                .Include(c => c.Services) // Ensure services are loaded
                .Where(c => c.Services.Any(s => s.ScheduledDate.Date >= today.Date && s.ScheduledDate.Date <= futureDate.Date && !s.IsCompleted))
                .ToListAsync();

            // Also consider cars whose *next calculated service date* falls within the window,
            // even if no explicit Service record exists yet.
            // This part of the logic might need to be refined if 'Service' entities are always pre-created.
            // For now, we rely on explicitly scheduled services.
            return carsWithServices;
        }

        //public override async Task<Car?> GetByIdAsync(Guid id)
        //{
        //    // When getting a car by ID, we might need its related rentals and services for availability checks
        //    return await _context.Cars
        //        .Include(c => c.Rentals)
        //        .Include(c => c.Services)
        //        .FirstOrDefaultAsync(c => c.Id == id);
        //}
    }
}