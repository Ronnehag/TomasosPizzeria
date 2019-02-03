using System.ComponentModel.DataAnnotations;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Models.ViewModels
{
    public class KundViewModel
    {
        public Kund Kund { get; set; }

        [Required(ErrorMessage = "Användarnamn är obligatoriskt")]
        [Display(Name = "Användarnamn")]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "Användarnamn får inte överstiga 20 karaktärer")]
        public string AnvandarNamn { get; set; }

        [Required(ErrorMessage = "Lösenord är obligatoriskt")]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        [StringLength(20, ErrorMessage = "Lösenord får inte överstiga 20 karaktärer")]
        public string Losenord { get; set; }

        [Required(ErrorMessage = "Validera ditt lösenord")]
        [Compare("Losenord", ErrorMessage = "Lösenord matchar inte")]
        [Display(Name = "Bekräfta lösenord")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
