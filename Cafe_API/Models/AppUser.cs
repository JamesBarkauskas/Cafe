using Microsoft.AspNetCore.Identity;

namespace Cafe_API.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
