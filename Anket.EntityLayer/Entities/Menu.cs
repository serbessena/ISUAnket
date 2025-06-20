using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Entities
{
    public class Menu
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Menü adı zorunludur")]
        [Display(Name ="Menü Adı")]
        [StringLength(1000)]
        [MaxLength(1000,ErrorMessage ="Menü adı en fazla 1000 karakter uzunluğunda olmadır")]
        public string MenuAdi { get; set; }

        [Required(ErrorMessage ="Menü linki zorunludur!")]
        [Display(Name ="Menü Bağlantısı")]
        [StringLength(1000)]
        [MaxLength(1000,ErrorMessage ="Menü bağlantısı en fazla 1000 karakter uzunluğunda olmalıdır")]
        public string Url { get; set; }

        [Display(Name ="Menü Resmi")]
        [StringLength(1000)]
        public string? Icon { get; set; }

        [Display(Name ="Menü Sırası")]
        public int Sira { get; set; }

        [Display(Name ="Durum")]
        public bool AktifMi { get; set; }=true;

        public List<MenuRol> MenuRoller { get; set; }
    }
}
