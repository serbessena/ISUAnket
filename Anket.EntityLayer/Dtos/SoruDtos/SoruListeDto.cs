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
        public Kullanici OlusturanKullanici { get; set; }

        [Display(Name = "Aktif mi ?")]
        public bool AktifMi { get; set; }

        [Display(Name = "Ait Olduğu Anket")]
        public int? AnketId { get; set; }
        public Anket? Anket { get; set; }
    }
}
