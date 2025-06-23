// CarRentalSystem.Application/Services/CarService.cs
using CarRentalSystem.Application.DTOs;
using CarRentalSystem.Application.Interfaces;
using CarRentalSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IServiceRepository _serviceRepository;

        public CarService(ICarRepository carRepository, IRentalRepository rentalRepository, IServiceRepository serviceRepository)
        {
            _carRepository = carRepository;
            _rentalRepository = rentalRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task<IEnumerable<AvailableCarDto>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate, string? type, string? model)
        {
            // Get all cars that match the type/model filter first
            var allCars = await _carRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(type))
            {
                allCars = allCars.Where(c => c.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(model))
            {
                allCars = allCars.Where(c => c.Model.Equals(model, StringComparison.OrdinalIgnoreCase));
            }

            var availableCars = new List<AvailableCarDto>();

            foreach (var car in allCars)
            {
                // Get rentals and services for the current car within a broader period to check conflicts
                var carRentals = await _rentalRepository.GetRentalsByCarAndPeriodAsync(car.Id, startDate.AddDays(-2), endDate.AddDays(2)); // Check surrounding days for next-day rule
                var carServices = await _serviceRepository.GetServicesByCarAndPeriodAsync(car.Id, startDate.AddMonths(-2), endDate.AddMonths(2)); // Check service history/future

                if (car.IsAvailable(startDate, endDate, carRentals, carServices))
                {
                    availableCars.Add(new AvailableCarDto
                    {
                        CarId = car.Id,
                        Type = car.Type,
                        Model = car.Model,
                        LastServiceDate = car.LastServiceDate
                    });
                }
            }

            return availableCars;
        }

        public async Task<CarResponseDto?> GetCarByIdAsync(Guid carId)
        {
            var car = await _carRepository.GetByIdAsync(carId);
            if (car == null) return null;

            return new CarResponseDto
            {
                Id = car.Id,
                Type = car.Type,
                Model = car.Model,
                LastServiceDate = car.LastServiceDate
            };
        }
    }
}