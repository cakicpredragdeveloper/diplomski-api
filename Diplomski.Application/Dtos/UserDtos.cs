using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Application.Dtos
{
    public class UserDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        public string IdentityId { get; set; }

        public int LocationId { get; set; }
        //public LocationDto Location { get; set; }
        public bool ShowMyOnlineStatus { get; set; }
        public bool ShowAllMyAds { get; set; }
        public bool AllowEmailNotifications { get; set; }
        public bool AllowAddressBook { get; set; }
        public int AdsCount { get; set; }
    }
    public class UserWithEmailDto : UserDto
    {
        public string Email { get; set; }

    }
}
