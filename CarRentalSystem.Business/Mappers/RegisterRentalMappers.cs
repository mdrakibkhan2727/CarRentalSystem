using CarRentalSystem.Business.DTOs.Rental;
using CarRentalSystem.Data.Models;

namespace CarRentalSystem.Business.Mappers
{
    public static class RegisterRentalMappers
    {
        public static RegisterRentalDto ToRegisterRentalDto(this Rental rental)
        {
            return new RegisterRentalDto
            {
                Id = rental.Id,
                FullName = rental.FullName,
                Address = rental.Address,
                CarModel = rental.CarModel,
                CarType = rental.CarType,
                StartDate = rental.StartDate,
                EndDate = rental.EndDate
            };
        }

        public static Rental ToRegisterRentalFromCreateDTO(this CreateRegisterRentalRequestDto registerRentalDto)
        {

            return new Rental
            { 
                FullName = registerRentalDto.FullName,
                Address = registerRentalDto.Address,
                Phone = registerRentalDto.Phone,
                CarModel = registerRentalDto.CarModel,
                CarType = registerRentalDto.CarType,
                StartDate = registerRentalDto.StartDate,
                EndDate = registerRentalDto.EndDate
            };
        }

        public static Rental ToRegisterRentalFromUpdateDTO(this UpdateRegisterRentalDto registerRentalDto, int id)
        {
            return new Rental
            {
                FullName = registerRentalDto.FullName,
                Address = registerRentalDto.Address,
                Phone = registerRentalDto.Phone,
                CarModel = registerRentalDto.CarModel,
                CarType = registerRentalDto.CarType,
                StartDate = registerRentalDto.StartDate,
                EndDate = registerRentalDto.EndDate
            };
        }
    }
}
