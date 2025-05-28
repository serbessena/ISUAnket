using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Dtos.SoruDtos
{
    public class SoruListeDto
    {
        public int Id { get; set; }

        [Display(Name = "Soru Metni")]
        public string SoruMetni { get; set; }

        [Display(Name = "Oluşturan Kullanıcı")]
        public int OlusturanKullaniciId { get; set; }

        [Display(Name = "Oluşturan Kullanıcı Adı")]
        public string OlusturanKullaniciAd { get; set; } 
        public string OlusturanKullaniciSoyad { get; set; } 

        [Display(Name = "Aktif mi ?")]
        public bool AktifMi { get; set; }

        [Display(Name = "Ait Olduğu Anket")]
        public int? AnketId { get; set; }

        [Display(Name = "Anket Adı")]
        public string? AnketAdi { get; set; } // örneğin "2025 Yılı Memnuniyet Anketi"
    }
}
