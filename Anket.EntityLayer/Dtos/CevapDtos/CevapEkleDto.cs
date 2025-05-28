using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Dtos.CevapDtos
{
    public class CevapEkleDto
    {
        [Required(ErrorMessage = "Çalışma birimi boş geçilemez.")]
        [Display(Name = "Çalışma Birimi")]
        public int Birim { get; set; }

        [Required(ErrorMessage = "Verilen cevap boş geçilemez.")]
        [StringLength(600, ErrorMessage = "Verilen cevap 600 karakterden fazla olamaz")]
        [MinLength(4, ErrorMessage = "Verilen cevap için minimum 4 karakter girilmesi gerekmektedir.")]
        [Display(Name = "Verilen Cevap")]
        public string VerilenCevap { get; set; }

        [Display(Name = "Cevaplanma Tarihi")]
        public DateTime CevapTarihi { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Soru bilgisi boş geçilemez.")]
        [Display(Name = "Bağlı Olduğu Soru")]
        public int SoruId { get; set; }
    }
}
