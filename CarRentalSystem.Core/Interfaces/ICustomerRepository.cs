// CarRentalSystem.Core/Interfaces/ICustomerRepository.cs
using CarRentalSystem.Core.Models;
using System;
using System.Threading.Tasks;

namespace CarRentalSystem.Core.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer?> GetByCustomerIdStringAsync(string customerIdString);
    }
}