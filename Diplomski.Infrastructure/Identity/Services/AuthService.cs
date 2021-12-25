using Diplomski.Application.Dtos;
using Diplomski.Application.Interfaces.ThirdPartyContracts;
using Diplomski.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplomski.Application.Exceptions;
using Microsoft.Extensions.Options;
using Diplomski.Infrastructure.Identity.Options;

namespace Diplomski.Infrastructure.Identity.Services
{
    public class AuthService : IAuthService
    {
        //private readonly IStringLocalizer<AuthService> _stringLocalizer;
        private readonly UserManager<IdentityAppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly JwtOption _jwtOption;


        public AuthService(UserManager<IdentityAppUser> userManager, ITokenService tokenService, IOptions<JwtOption> jwtOption)
        {
            //this._stringLocalizer = stringLocalizer;
            this._userManager = userManager;
            this._tokenService = tokenService;
            this._jwtOption = jwtOption.Value;
        }

        public async Task<string> CreateIdentityAsync(RegisterDto registerDto)
        {
            var registeredUser = await this._userManager.FindByNameAsync(registerDto.Email);
            if (registeredUser != null)
                return await Task.FromResult("User alredy registered");

            var user = new IdentityAppUser { UserName = registerDto.Email, Email = registerDto.Email };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var message = result.Errors
                    .Select(e => e.Description)
                    .Aggregate((current, next) => $"{current} {next}");

                throw new Exception(message);
            }

            return user.Id;

        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            var identityUser = _userManager
                .Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefault(u => u.Email == loginDto.Email);

            if (identityUser is null)
            {
                //throw new ApiException(string.Format(_stringLocalizer["UserWithEmailNotFound"], loginDto.Email), 404);
                throw new ApiException("UserWithEmailNotFound: " + loginDto.Email, 404);
            }

            if (!await _userManager.CheckPasswordAsync(identityUser, loginDto.Password))
            {
                //throw new ApiException(_stringLocalizer["CredentialsNotValid"], 400);
                throw new ApiException("CredentialsNotValid", 400);
            }

            var roles = await _userManager.GetRolesAsync(identityUser);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var accessToken = _tokenService.GenerateAccessToken(identityUser.UserName, identityUser.Id, roles);

            if (identityUser.RefreshTokens is null)
            {
                identityUser.RefreshTokens = new List<RefreshToken>();
            }

            identityUser.RefreshTokens.Add(new RefreshToken { Token = refreshToken, Expires = _jwtOption.RefreshTokenExpiration });//Ovde bi brisao sve prethodne refreshToken-e jer na skavi login dodoajemo novi refreshToken
            await _userManager.UpdateAsync(identityUser);

            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiration = _jwtOption.Expiration
            };
        }

        public async Task<TokenDto> RevokeAsync(RevokeTokenDto revokeTokenDto)
        {
            var userNameClaim = _tokenService.GetUserNameClaimFromToken(revokeTokenDto.AccessToken);

            if (userNameClaim is null)
            {
                throw new ApiException("AccessTokenNotValid", 401);
            }

            var identityUser = await _userManager
                .Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.UserName == userNameClaim.Value);

            if (identityUser is null)
            {
                throw new ApiException("UserWithUserNameNotFound" + userNameClaim.Value, 404);
            }

            var refreshToken = identityUser.RefreshTokens.FirstOrDefault(t => t.Token == revokeTokenDto.RefreshToken && t.Active);
            if (refreshToken is null)
            {
                throw new ApiException("RefreshTokenNotValid", 401);
            }

            var roles = await _userManager.GetRolesAsync(identityUser);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var newAccessToken = _tokenService.GenerateAccessToken(identityUser.UserName, identityUser.Id, roles);

            identityUser.RefreshTokens.Remove(refreshToken); 
            identityUser.RefreshTokens.Add(new RefreshToken { Token = newRefreshToken, Expires = _jwtOption.RefreshTokenExpiration});
            await _userManager.UpdateAsync(identityUser);

            return new TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Expiration = _jwtOption.Expiration
            };

        }

        public async Task<string> FindEmailByIdAsync(string identityId)
        {
            var identityUser = await _userManager.FindByIdAsync(identityId);

            if (identityUser is null)
            {
                throw new ApiException("UserWithIdNotFound" + identityId, 404);
            }

            return identityUser.Email;
        }
    }
}
