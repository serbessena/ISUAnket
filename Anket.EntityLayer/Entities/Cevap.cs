using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Entities
{
    public class Cevap
    {
        [Key]
        public int Id { get; set; }
        //public string IpAdress { get; set; }

        [Display(Name ="Cevap")]
        [StringLength(1500)]
        [MaxLength(1500,ErrorMessage ="Cevap 1500 karakterden fazla olamaz")]
        public string VerilenCevap { get; set; }

        [Display(Name ="Cevap Tarihi")]
        public DateTime CevapTarihi { get; set; } = DateTime.Now;

        [Display(Name ="Aktif mi?")]
        public bool AktifMi { get; set; } = true;

        [Display(Name ="Soru")]
        public int SoruId { get; set; }
        public Soru Soru { get; set; }

        [Required(ErrorMessage ="Birim seçimi zorunludur!")]
        [Display(Name ="Birim")]
        public int BirimId { get; set; }
        public Birim Birim { get; set; }
    }
}
