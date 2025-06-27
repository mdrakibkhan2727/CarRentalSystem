using CarRentalSystem.Business.Repositories.IRepository;
using CarRentalSystem.Data.DataContext;
using CarRentalSystem.Data.Models;

namespace CarRentalSystem.Business.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        public ServiceRepository(ApplicationDbContext context)
        {
        }

    }
}