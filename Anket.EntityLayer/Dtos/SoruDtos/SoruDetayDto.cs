using ISUAnket.EntityLayer.Entities;
using ISUAnket.EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Dtos.SoruDtos
{
    public class SoruDetayDto
    {
        public int Id { get; set; }
        [Display(Name = "Soru Metni")]
        public string SoruMetni { get; set; }

        [Display(Name = "Soru Tipi")]
        public SoruTipiEnum? SoruTipi { get; set; }

        [Display(Name = "Soru Seçenekleri")]
        public string? SoruSecenekleri { get; set; }

        [Display(Name = "Oluşturan Kullanıcı")]
        public int OlusturanKullaniciId { get; set; }
        public Kullanici OlusturanKullanici { get; set; }

        [Display(Name = "Oluşturma Tarihi")]
        public DateTime OlusturmaTarihi { get; set; }

        [Display(Name = "Düzenleyen Kullanıcı")]
        public int? DuzenleyenKullaniciId { get; set; }
        public Kullanici? DuzenleyenKullanici { get; set; }

        [Display(Name = "Düzenleme Tarihi")]
        public DateTime? DuzenlenmeTarihi { get; set; }

        [Display(Name = "Aktif mi ?")]
        public bool AktifMi { get; set; }

        [Display(Name = "Zorunlu mu ?")]
        public bool? ZorunluMu { get; set; }

        [Display(Name = "Ait Olduğu Anket")]
        public int? AnketId { get; set; }
        public Anket? Anket { get; set; }

    }
}
