// CarRentalSystem.Core/Models/Car.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarRentalSystem.Core.Models
{
    public class Car
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Type { get; set; } = string.Empty; // e.g., "SUV", "Sedan"
        public string Model { get; set; } = string.Empty; // e.g., "CR-V", "Civic"
        public DateTime LastServiceDate { get; set; } // Tracks when the last service was done

        // Navigation property for Entity Framework
        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
        public ICollection<Service> Services { get; set; } = new List<Service>();

        // Business logic methods
        public bool IsAvailable(DateTime startDate, DateTime endDate, IEnumerable<Rental> existingRentals, IEnumerable<Service> existingServices)
        {
            // A car can only be assigned to one customer at a time.
            // After the rental is completed, that car cannot be rented for the next day.
            // All cars have a service every 2 months that lasts 2 days.

            // 1. Check for overlapping rentals
            if (existingRentals.Any(r =>
                (startDate < r.EndDate && endDate > r.StartDate)))
            {
                return false;
            }

            // 2. Check for next-day rental restriction
            if (existingRentals.Any(r =>
                r.EndDate.Date == startDate.AddDays(-1).Date)) // If previous rental ends the day before new start
            {
                return false;
            }

            // 3. Check for service schedule conflicts
            // Calculate potential service dates (every 2 months from LastServiceDate)
            var currentServiceIntervalStart = LastServiceDate.AddMonths(2);
            while (currentServiceIntervalStart <= endDate.AddDays(2)) // Check up to just after the rental end + service duration
            {
                var serviceEndDate = currentServiceIntervalStart.AddDays(2); // Service lasts 2 days
                if (existingServices.Any(s => s.CarId == Id && s.ScheduledDate.Date == currentServiceIntervalStart.Date && !s.IsCompleted))
                {
                    // If there's an explicit upcoming service record, use that
                    if ((startDate < serviceEndDate && endDate > currentServiceIntervalStart))
                    {
                        return false;
                    }
                }
                else
                {
                    // Otherwise, consider the potential automatic service based on LastServiceDate
                    // If the service period overlaps with the proposed rental
                    if ((startDate < serviceEndDate && endDate > currentServiceIntervalStart))
                    {
                        // We assume a car *will* go into service at this time if no explicit service exists
                        // This is a simplification; in a real system, services would be explicitly scheduled
                        return false;
                    }
                }
                currentServiceIntervalStart = currentServiceIntervalStart.AddMonths(2); // Move to next potential service interval
            }

            return true;
        }
    }
}