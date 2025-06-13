using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Entities
{
    public class Birim
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Birim adı zorunludur!")]
        [StringLength(200)]
        [MaxLength(200,ErrorMessage ="Birim adı en fazla 200 karakter olmalıdır")]
        [MinLength(5,ErrorMessage ="Birim adı en az 5 karakter uzunluğunda olmalıdır")]
        public string Ad { get; set; }

        [Display(Name = "Durum")]
        public bool AktifMi { get; set; }=false;

        public List<Cevap> Cevaplar { get; set; }
    }
}
