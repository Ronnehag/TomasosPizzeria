using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TomasosPizzeria.Models.Entities;

namespace TomasosPizzeria.Models
{
    public class KundMetaData
    {
        [Required(ErrorMessage = "Namn är obligatoriskt")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Namn får inte överstiga 100 karaktärer")]
        public object Namn { get; set; }

        [Required(ErrorMessage = "Gatuadress är obligatoriskt")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Gatuadress får inte överstiga 50 karaktärer")]
        public object Gatuadress { get; set; }

        [Required(ErrorMessage = "Postnr är obligatoriskt")]
        [DataType(DataType.PostalCode)]
        [StringLength(20, ErrorMessage = "Postnr får inte överstiga 20 karaktärer")]
        public object Postnr { get; set; }

        [Required(ErrorMessage = "Postort är obligatoriskt")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Postort får inte överstiga 100 karaktärer")]
        public object Postort { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "Email får inte överstiga 50 karaktärer")]
        public object Email { get; set; }

        [DataType((DataType.PhoneNumber))]
        [StringLength(50, ErrorMessage = "Telefonnummer får inte överstiga 50 karaktärer")]
        [Display(Name = "Telefonnummer")]
        public object Telefon { get; set; }

        [Required(ErrorMessage = "Användarnamn är obligatoriskt")]
        [Display(Name = "Användarnamn")]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "Användarnamn får inte överstiga 20 karaktärer")]
        public object AnvandarNamn { get; set; }

        [Required(ErrorMessage = "Lösenord är obligatoriskt")]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        [StringLength(20, ErrorMessage = "Lösenord får inte överstiga 20 karaktärer")]
        public object Losenord { get; set; }


    }
}
