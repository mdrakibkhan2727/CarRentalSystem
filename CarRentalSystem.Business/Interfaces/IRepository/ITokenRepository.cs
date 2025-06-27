using CarRentalSystem.Data.Models;

namespace CarRentalSystem.Business.Repositories.IRepository
{
    public interface ITokenRepository
    {
        string CreateToken(AppUser user);
    }
}
