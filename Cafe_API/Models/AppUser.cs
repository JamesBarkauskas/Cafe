using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe_API.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        [NotMapped]
        public string Role { get; set; }

    }
}
