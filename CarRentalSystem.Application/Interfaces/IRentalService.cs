// CarRentalSystem.Application/Interfaces/IRentalService.cs
using CarRentalSystem.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Interfaces
{
    public interface IRentalService
    {
        Task<RentalResponseDto> RegisterRentalAsync(RegisterRentalDto rentalDto);
        Task<RentalResponseDto> ModifyReservationAsync(Guid rentalId, UpdateRentalDto updateDto);
        Task CancelRentalAsync(Guid rentalId);
        Task<RentalResponseDto?> GetRentalByIdAsync(Guid rentalId);
    }
}