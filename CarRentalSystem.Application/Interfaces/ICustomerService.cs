// CarRentalSystem.Application/Interfaces/ICustomerService.cs
using CarRentalSystem.Application.DTOs;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerResponseDto> RegisterCustomerAsync(CustomerDto customerDto);
        Task<CustomerResponseDto?> GetCustomerByIdStringAsync(string customerIdString);
    }
}