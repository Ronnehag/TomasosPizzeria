using Microsoft.AspNetCore.Identity;

namespace TomasosPizzeria.IdentityData
{
    public class AppUser : IdentityUser
    {
        public string Email { get; set; }
    }
}
