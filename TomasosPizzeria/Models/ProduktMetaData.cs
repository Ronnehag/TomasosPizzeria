using System.ComponentModel.DataAnnotations;

namespace TomasosPizzeria.Models
{
    public class ProduktMetaData
    {
        [Required]
        [StringLength(50, ErrorMessage = "Namn får inte överstiga 50 bokstäver")]
        public string ProduktNamn { get; set; }
    }
}
