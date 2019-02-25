using System.ComponentModel.DataAnnotations;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Models.ViewModels
{
    public class KundEditDetailsViewModel
    {
        public Kund Kund { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Du måste ange ditt lösenord")]
        [Display(Name="Nuvarande lösenord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name="Nytt lösenord")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Lösenord stämmer inte")]
        [Display(Name="Bekräfta lösenord")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Text)]
        [Display(Name="Användarnamn")]
        public string UserName { get; set; }
    }
}
