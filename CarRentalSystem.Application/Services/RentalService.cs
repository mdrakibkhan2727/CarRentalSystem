// CarRentalSystem.Application/Services/RentalService.cs
using CarRentalSystem.Application.DTOs;
using CarRentalSystem.Application.Interfaces;
using CarRentalSystem.Core.Interfaces;
using CarRentalSystem.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly ICarRepository _carRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IServiceRepository _serviceRepository;

        public RentalService(IRentalRepository rentalRepository, ICarRepository carRepository, ICustomerRepository customerRepository, IServiceRepository serviceRepository)
        {
            _rentalRepository = rentalRepository;
            _carRepository = carRepository;
            _customerRepository = customerRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task<RentalResponseDto> RegisterRentalAsync(RegisterRentalDto rentalDto)
        {
            if (rentalDto.StartDate.Date > rentalDto.EndDate.Date)
            {
                throw new ArgumentException("Start date cannot be after end date.");
            }

            var customer = await _customerRepository.GetByCustomerIdStringAsync(rentalDto.CustomerIdString);
            if (customer == null)
            {
                throw new InvalidOperationException($"Customer with ID '{rentalDto.CustomerIdString}' not found.");
            }

            var car = await _carRepository.GetByIdAsync(rentalDto.CarId);
            if (car == null)
            {
                throw new InvalidOperationException($"Car with ID '{rentalDto.CarId}' not found.");
            }

            // Check availability for the car
            var existingRentals = await _rentalRepository.GetRentalsByCarAndPeriodAsync(car.Id, rentalDto.StartDate.AddDays(-2), rentalDto.EndDate.AddDays(2));
            var existingServices = await _serviceRepository.GetServicesByCarAndPeriodAsync(car.Id, rentalDto.StartDate.AddMonths(-2), rentalDto.EndDate.AddMonths(2));

            if (!car.IsAvailable(rentalDto.StartDate, rentalDto.EndDate, existingRentals, existingServices))
            {
                throw new InvalidOperationException($"Car '{car.Model}' ({car.Type}) is not available for the requested period.");
            }

            var newRental = new Rental
            {
                CustomerId = customer.Id,
                CarId = car.Id,
                StartDate = rentalDto.StartDate.Date, // Store as date only
                EndDate = rentalDto.EndDate.Date      // Store as date only
            };

            await _rentalRepository.AddAsync(newRental);

            return new RentalResponseDto
            {
                RentalId = newRental.Id,
                CustomerId = customer.Id,
                CustomerFullName = customer.FullName,
                CarId = car.Id,
                CarType = car.Type,
                CarModel = car.Model,
                StartDate = newRental.StartDate,
                EndDate = newRental.EndDate
            };
        }

        public async Task<RentalResponseDto> ModifyReservationAsync(Guid rentalId, UpdateRentalDto updateDto)
        {
            var existingRental = await _rentalRepository.GetByIdAsync(rentalId);
            if (existingRental == null)
            {
                throw new InvalidOperationException($"Rental with ID '{rentalId}' not found.");
            }

            if (updateDto.StartDate.Date > updateDto.EndDate.Date)
            {
                throw new ArgumentException("New start date cannot be after new end date.");
            }

            // Check if dates are in the past
            if (updateDto.StartDate.Date < DateTime.Today.Date)
            {
                throw new ArgumentException("New start date cannot be in the past.");
            }

            // Determine the car to check availability for (original or new)
            var targetCarId = updateDto.CarId ?? existingRental.CarId;
            var car = await _carRepository.GetByIdAsync(targetCarId);

            if (car == null)
            {
                throw new InvalidOperationException($"Target car with ID '{targetCarId}' not found.");
            }

            // For modification, we need to temporarily exclude the current rental from availability check
            // to allow modifications that don't conflict with *itself*.
            var allRentalsForCar = await _rentalRepository.GetRentalsByCarAndPeriodAsync(targetCarId, updateDto.StartDate.AddDays(-2), updateDto.EndDate.AddDays(2));
            var otherRentalsForCar = allRentalsForCar.Where(r => r.Id != rentalId).ToList(); // Exclude the rental being modified

            var existingServices = await _serviceRepository.GetServicesByCarAndPeriodAsync(targetCarId, updateDto.StartDate.AddMonths(-2), updateDto.EndDate.AddMonths(2));


            if (!car.IsAvailable(updateDto.StartDate, updateDto.EndDate, otherRentalsForCar, existingServices))
            {
                throw new InvalidOperationException($"Car '{car.Model}' ({car.Type}) is not available for the requested new period.");
            }

            // Apply changes
            existingRental.StartDate = updateDto.StartDate.Date;
            existingRental.EndDate = updateDto.EndDate.Date;
            if (updateDto.CarId.HasValue)
            {
                existingRental.CarId = updateDto.CarId.Value;
            }

            await _rentalRepository.UpdateAsync(existingRental);

            var customer = await _customerRepository.GetByIdAsync(existingRental.CustomerId); // Fetch customer for response DTO

            return new RentalResponseDto
            {
                RentalId = existingRental.Id,
                CustomerId = existingRental.CustomerId,
                CustomerFullName = customer?.FullName ?? "Unknown",
                CarId = existingRental.CarId,
                CarType = car.Type,
                CarModel = car.Model,
                StartDate = existingRental.StartDate,
                EndDate = existingRental.EndDate
            };
        }

        public async Task CancelRentalAsync(Guid rentalId)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            if (rental == null)
            {
                throw new InvalidOperationException($"Rental with ID '{rentalId}' not found.");
            }

            // Optional: Add business rules like "cannot cancel if rental has already started"
            if (rental.StartDate.Date <= DateTime.Today.Date)
            {
                throw new InvalidOperationException("Cannot cancel a rental that has already started or passed.");
            }

            await _rentalRepository.DeleteAsync(rental);
        }

        public async Task<RentalResponseDto?> GetRentalByIdAsync(Guid rentalId)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            if (rental == null) return null;

            var customer = await _customerRepository.GetByIdAsync(rental.CustomerId);
            var car = await _carRepository.GetByIdAsync(rental.CarId);

            return new RentalResponseDto
            {
                RentalId = rental.Id,
                CustomerId = rental.CustomerId,
                CustomerFullName = customer?.FullName ?? "N/A",
                CarId = rental.CarId,
                CarType = car?.Type ?? "N/A",
                CarModel = car?.Model ?? "N/A",
                StartDate = rental.StartDate,
                EndDate = rental.EndDate
            };
        }
    }
}