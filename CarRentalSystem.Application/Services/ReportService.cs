// CarRentalSystem.Application/Services/ReportService.cs
using CarRentalSystem.Application.DTOs;
using CarRentalSystem.Application.Interfaces;
using CarRentalSystem.Core.Interfaces;
using CarRentalSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly ICarRepository _carRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IRentalRepository _rentalRepository;

        public ReportService(ICarRepository carRepository, IServiceRepository serviceRepository, IRentalRepository rentalRepository)
        {
            _carRepository = carRepository;
            _serviceRepository = serviceRepository;
            _rentalRepository = rentalRepository;
        }

        public async Task<IEnumerable<UpcomingServiceDto>> GetUpcomingServicesAsync(DateTime today, int daysAhead = 14)
        {
            var cars = await _carRepository.GetCarsWithUpcomingServicesAsync(today, daysAhead);
            var upcomingServices = new List<UpcomingServiceDto>();

            foreach (var car in cars)
            {
                // This logic needs to be robust. The Car.IsAvailable logic for services is basic.
                // We need to properly query scheduled services from the Service entity.
                var carServices = await _serviceRepository.GetServicesByCarAndPeriodAsync(car.Id, today, today.AddDays(daysAhead));

                foreach (var service in carServices.Where(s => !s.IsCompleted))
                {
                    upcomingServices.Add(new UpcomingServiceDto
                    {
                        CarId = car.Id,
                        Model = car.Model,
                        Type = car.Type,
                        ServiceDate = service.ScheduledDate
                    });
                }
            }
            return upcomingServices.OrderBy(s => s.ServiceDate);
        }

        public async Task<CarUtilizationReportDto> GetCarUtilizationStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be after end date for utilization report.");
            }

            var rentals = await _rentalRepository.GetAllRentalsInDateRangeAsync(startDate, endDate);
            var cars = (await _carRepository.GetAllAsync()).ToList(); // Get all cars to map IDs to types/models

            var totalRentalsOverall = rentals.Count();
            if (totalRentalsOverall == 0)
            {
                return new CarUtilizationReportDto
                {
                    MostRentedCarType = "N/A",
                    UtilizationPercentage = 0,
                    CarTypeBreakdown = new List<CarUtilizationDto>(),
                    TotalRentalsOverall = 0
                };
            }

            var carTypeCounts = rentals
                .Join(cars,
                      rental => rental.CarId,
                      car => car.Id,
                      (rental, car) => car.Type)
                .GroupBy(type => type)
                .Select(g => new CarUtilizationDto
                {
                    Type = g.Key,
                    TotalRentals = g.Count(),
                    Percentage = Math.Round((double)g.Count() / totalRentalsOverall * 100, 2)
                })
                .OrderByDescending(x => x.Percentage)
                .ToList();

            var mostRented = carTypeCounts.FirstOrDefault();

            return new CarUtilizationReportDto
            {
                MostRentedCarType = mostRented?.Type ?? "N/A",
                UtilizationPercentage = mostRented?.Percentage ?? 0,
                CarTypeBreakdown = carTypeCounts,
                TotalRentalsOverall = totalRentalsOverall
            };
        }
    }
}