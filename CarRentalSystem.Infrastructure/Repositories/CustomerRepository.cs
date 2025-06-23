// CarRentalSystem.Infrastructure/Repositories/CustomerRepository.cs
using CarRentalSystem.Core.Interfaces;
using CarRentalSystem.Core.Models;
using CarRentalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CarRentalSystem.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Customer?> GetByCustomerIdStringAsync(string customerIdString)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerIdString == customerIdString);
        }
    }
}