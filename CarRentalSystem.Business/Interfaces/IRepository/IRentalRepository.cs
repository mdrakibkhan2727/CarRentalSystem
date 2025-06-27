using CarRentalSystem.Business.DTOs.Rental;
using CarRentalSystem.Business.Helpers;
using CarRentalSystem.Data.Models;

namespace CarRentalSystem.Business.Repositories.IRepository
{
    public interface IRentalRepository
    {
        Task<List<Rental>> GetAllAsync(QueryRegisterRentalObject query);
        Task<Rental> CreateAsync(CreateRegisterRentalRequestDto rentalDto);
        Task<Rental> UpdateAsync(int id, UpdateRegisterRentalDto rentalDto);
        Task<Rental> DeleteAsync(int id);
        Task<Rental?> GetByIdAsync(int id);
    }
}