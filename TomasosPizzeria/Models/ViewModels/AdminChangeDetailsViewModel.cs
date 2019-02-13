using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TomasosPizzeria.Models.ViewModels
{
    public class AdminChangeDetailsViewModel
    {
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Ange en email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Ange nuvarande lösenord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Lösenord matchar inte")]
        public string ConfirmPassword { get; set; }
    }
}
