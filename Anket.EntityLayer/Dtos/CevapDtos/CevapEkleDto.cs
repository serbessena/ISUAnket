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
        public int Id { get; set; }
        //public string IpAdress { get; set; }

        [Required(ErrorMessage = "Soru metni boş geçilemez.")]
        [Display(Name = "Çalışma Birimi")]
        public int Birim { get; set; }

        [StringLength(600, ErrorMessage = "Verilen cevap 600 karakterden fazla olamaz")]
        [MinLength(4, ErrorMessage = "Verilen cevap için minimum 4 karakter girilmesi gerekmektedir.")]
        [Display(Name = "Verilen Cevap")]
        public string VerilenCevap { get; set; }

        [Display(Name = "Cevaplanma Tarihi")]
        public DateTime CevapTarihi { get; set; }

        [Display(Name = "Bağlı olduğu soru")]
        public int SoruId { get; set; }
        public Soru Soru { get; set; }
    }
}
