using Diplomski.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Application.Interfaces
{
    public interface IAccountService
    {
        Task RegisterAsync(RegisterDto registerDto);
        Task<TokenDto> LoginAsync(LoginDto loginDto);
        Task<TokenDto> RevokeTokenAsync(RevokeTokenDto revokeTokenDto);
        Task<UserWithEmailDto> GetAuthenticatedUserAsync(string token);
    }
}
