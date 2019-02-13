using System.ComponentModel.DataAnnotations;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Models.ViewModels
{
    public class KundEditDetailsViewModel
    {
        public Kund Kund { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Du måste ange ditt lösenord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Lösenord stämmer inte")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Text)]
        public string UserName { get; set; }
    }
}
