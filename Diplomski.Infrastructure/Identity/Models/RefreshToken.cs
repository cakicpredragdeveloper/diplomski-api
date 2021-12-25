using System;
using System.Collections.Generic;
using System.Text;

namespace Diplomski.Infrastructure.Identity.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public string IdentityAppUserId { get; set; }
        public string RemoteIpAddress { get; set; }
        public bool Active => DateTime.Now <= Expires;
    }
}
