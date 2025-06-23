// CarRentalSystem.Application/Services/CustomerService.cs
using CarRentalSystem.Application.DTOs;
using CarRentalSystem.Application.Interfaces;
using CarRentalSystem.Core.Interfaces;
using CarRentalSystem.Core.Models;
using System;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerResponseDto> RegisterCustomerAsync(CustomerDto customerDto)
        {
            var existingCustomer = await _customerRepository.GetByCustomerIdStringAsync(customerDto.CustomerIdString);
            if (existingCustomer != null)
            {
                throw new InvalidOperationException($"Customer with ID '{customerDto.CustomerIdString}' already exists.");
            }

            var customer = new Customer
            {
                CustomerIdString = customerDto.CustomerIdString,
                FullName = customerDto.FullName,
                Address = customerDto.Address
            };

            await _customerRepository.AddAsync(customer);

            return new CustomerResponseDto
            {
                Id = customer.Id,
                CustomerIdString = customer.CustomerIdString,
                FullName = customer.FullName,
                Address = customer.Address
            };
        }

        public async Task<CustomerResponseDto?> GetCustomerByIdStringAsync(string customerIdString)
        {
            var customer = await _customerRepository.GetByCustomerIdStringAsync(customerIdString);
            if (customer == null) return null;

            return new CustomerResponseDto
            {
                Id = customer.Id,
                CustomerIdString = customer.CustomerIdString,
                FullName = customer.FullName,
                Address = customer.Address
            };
        }
    }
}