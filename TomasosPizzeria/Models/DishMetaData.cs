using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TomasosPizzeria.Models
{
    public class DishMetaData
    {
        [StringLength(50, ErrorMessage = "Namnet får inte överstiga 50 bokstäver")]
        [Required(ErrorMessage = "Namn måste anges")]
        public string MatrattNamn { get; set; }

        [StringLength(200, ErrorMessage = "Beskrivning får inte överstiga 200 bokstäver")]
        public string Beskrivning { get; set; }

        [Range(1,int.MaxValue, ErrorMessage = "Pris måste anges")]
        public int Pris { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Kategori måste anges")]
        public int MatrattTyp { get; set; }

    }
}
