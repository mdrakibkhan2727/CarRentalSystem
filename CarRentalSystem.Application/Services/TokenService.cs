using CarRentalSystem.Application.Interfaces;
using CarRentalSystem.Core.Interfaces;
using CarRentalSystem.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        public TokenService(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }
        public string CreateToken(AppUser user)
        {
            var token = _tokenRepository.CreateToken(user);

            return token.ToString();
        }
    }
}
