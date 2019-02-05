using System.Collections.Generic;
using TomasosPizzeria.IdentityData;

namespace TomasosPizzeria.Models.ViewModels
{
    public class AdminUserViewModel
    {
        public List<AppUser> RegularUsers { get; set; }
        public List<AppUser> PremiumUsers { get; set; }
    }
}
