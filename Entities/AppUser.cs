using Microsoft.AspNetCore.Identity;
using System.Data.Common;

namespace ShopAppAPI.Entities
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
