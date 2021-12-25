using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Diplomski.Infrastructure.Identity.Models
{
    public class IdentityAppUser : IdentityUser
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
