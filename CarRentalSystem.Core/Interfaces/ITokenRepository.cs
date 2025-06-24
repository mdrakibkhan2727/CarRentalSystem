using CarRentalSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Core.Interfaces
{
    public interface ITokenRepository
    {
        string CreateToken(AppUser user);
    }
}
