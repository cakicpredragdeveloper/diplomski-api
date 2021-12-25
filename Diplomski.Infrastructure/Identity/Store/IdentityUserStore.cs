using Diplomski.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Diplomski.Infrastructure.Identity.Store
{
    //class IdentityUserStore: IUserStore<IdentityAppUser>, IUserPasswordStore<IdentityAppUser>
    //{
        //public void Dispose()
        //{ 
        //}

        //public Task<string> GetUserIdAsync(IdentityAppUser user, CancellationToken cancelationToken)
        //{
        //    return Task.FromResult(user.Id);
        //}

        //public Task<string> GetUserNameAsync(IdentityAppUser user, CancellationToken cancellationToken)
        //{
        //    return Task.FromResult(user.UserName);
        //}

        //public Task SetUserNameAsync(IdentityAppUser user, string UserName, CancellationToken cancellationToken)
        //{
        //    user.UserName = UserName;
        //    return Task.CompletedTask;
        //}

        //public Task<string> GetNormalizedUserNameAsync(IdentityAppUser user, CancellationToken cancellationToken)
        //{
        //    return Task.FromResult(user.NormalizedUserName);
        //}

        //public Task SetNormalizedUserNameAsync(IdentityAppUser user, string normalizedUserName, CancellationToken cancellationToken)
        //{
        //    user.UserName = normalizedUserName;
        //    return Task.CompletedTask;
        //}

        //public async Task<IdentityResult> CreateAsync(IdentityAppUser user, CancellationToken cancellationToken)
        //{
        //    IdentityAppUser user354;
        //    user354 = user;
        //    return IdentityResult.Success;
        //}

        //public async Task<IdentityResult> UpdateAsync(IdentityAppUser user, CancellationToken cancellationToken)
        //{
        //    return IdentityResult.Success;
        //}

        //public async Task<IdentityAppUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        //{
        //    IdentityAppUser user354 = new IdentityAppUser();
        //    user354.Id = userId;
        //    return user354;
        //}

        //public async Task<IdentityAppUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        //{
        //    //IdentityAppUser user354 = new IdentityAppUser();
        //    return null;
        //}

        //public async Task<IdentityResult> DeleteAsync(IdentityAppUser user, CancellationToken cancellationToken)
        //{
        //    return IdentityResult.Success;
        //}

        //public Task SetPasswordHashAsync(IdentityAppUser user, string passwordHash, CancellationToken cancellationToken)
        //{
        //    user.PasswordHash = passwordHash;
        //    return Task.CompletedTask;
        //}

        //public Task<string> GetPasswordHashAsync(IdentityAppUser user, CancellationToken cancellationToken)
        //{
        //    return Task.FromResult(user.PasswordHash);
        //}

        //public Task<bool> HasPasswordAsync(IdentityAppUser user, CancellationToken cancellationToken)
        //{
        //   return Task.FromResult(user.PasswordHash != null);
        //}
    //}
}
