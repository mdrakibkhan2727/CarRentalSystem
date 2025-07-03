using CarRentalSystem.Business.DTOs.Rental;
using CarRentalSystem.Business.Helpers;
using CarRentalSystem.Business.Mappers;
using CarRentalSystem.Business.Repositories.IRepository;
using CarRentalSystem.Data.DataContext;
using CarRentalSystem.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Business.Repositories
{
    public class RentalRepository :  IRentalRepository
    {
        private readonly ApplicationDbContext _dBContext;
        public RentalRepository(ApplicationDbContext dBContext) 
        {
            _dBContext = dBContext;
        }

        public async Task<List<Rental>> GetAllAsync(QueryRegisterRentalObject query)
        {
            // Rentals + Include Customer info
            var rentalsQuery = _dBContext.Rentals.AsQueryable();

            // Filter by Customer FullName
            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                rentalsQuery = rentalsQuery.Where(r => 
                  r.FullName.ToLower().Contains(query.SearchTerm.ToLower()) ||
                  r.Phone.ToLower().Contains(query.SearchTerm.ToLower())
                );
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("FullName", StringComparison.OrdinalIgnoreCase))
                {
                    rentalsQuery = query.IsDecsending
                        ? rentalsQuery.OrderByDescending(r => r.FullName)
                        : rentalsQuery.OrderBy(r => r.FullName);
                }
            }

            // Pagination
            var skip = (query.PageNumber - 1) * query.PageSize;
            rentalsQuery = rentalsQuery.Skip(skip).Take(query.PageSize);

            return await rentalsQuery.ToListAsync();
        }

        public async Task<Rental?> GetByIdAsync(int id)
        {
            var rentalModel =   await _dBContext.Rentals.FirstOrDefaultAsync(c => c.Id == id);
            var data = rentalModel.ToRegisterRentalDto();
            return rentalModel;
        }
        public async Task<Rental> CreateAsync(CreateRegisterRentalRequestDto rentalDto)
        {
            var rentalModel = rentalDto.ToRegisterRentalFromCreateDTO();
            await _dBContext.Rentals.AddAsync(rentalModel);
            await _dBContext.SaveChangesAsync();
            return rentalModel;
        }

        public async Task<Rental> DeleteAsync(int id)
        {
            var rentalModel = await _dBContext.Rentals.FirstOrDefaultAsync(s => s.Id == id);
            if (rentalModel == null)
            {
                return null;
            }

            _dBContext.Rentals.Remove(rentalModel);
            await _dBContext.SaveChangesAsync();
            return rentalModel;
        }

        public async Task<Rental> UpdateAsync(int id, UpdateRegisterRentalDto rentalDto)
        {
            var existingrentalModel = await _dBContext.Rentals.FirstOrDefaultAsync(s => s.Id == id);

            if (existingrentalModel == null)
            {
                return null;
            }

            existingrentalModel.FullName = rentalDto.FullName;
            existingrentalModel.Address = rentalDto.Address;
            existingrentalModel.CarType = rentalDto.CarType;
            existingrentalModel.CarModel = rentalDto.CarModel;
            existingrentalModel.StartDate = rentalDto.StartDate;
            existingrentalModel.EndDate = rentalDto.EndDate;

            await _dBContext.SaveChangesAsync();
            return existingrentalModel;
        }
    }
}