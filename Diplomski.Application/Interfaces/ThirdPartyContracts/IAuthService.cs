using Diplomski.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Application.Interfaces.ThirdPartyContracts
{
    public interface IAuthService
    {
        Task<string> CreateIdentityAsync(RegisterDto registerDto);

        Task<TokenDto> LoginAsync(LoginDto loginDto);
        Task<TokenDto> RevokeAsync(RevokeTokenDto revokeTokenDto);
        Task<string> FindEmailByIdAsync(string identityId);
    }
}
