using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Dtos.CevapDtos
{
    public class CevapListeDto
    {
        public int Id { get; set; }

        [Display(Name = "Çalışma Birimi")]
        public int Birim { get; set; }

        [Display(Name = "Verilen Cevap")]
        public string VerilenCevap { get; set; }

        [Display(Name = "Cevaplanma Tarihi")]
        public DateTime CevapTarihi { get; set; }

        [Display(Name = "Bağlı Olduğu Soru ID")]
        public int SoruId { get; set; }

        [Display(Name = "Soru Metni")]
        public string SoruMetni { get; set; } // Soru objesi yerine, sade bir alan
    }
}
