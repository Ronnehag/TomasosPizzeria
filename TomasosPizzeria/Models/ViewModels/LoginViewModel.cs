using System.ComponentModel.DataAnnotations;

namespace TomasosPizzeria.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Användarnamn")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }
    }
}
