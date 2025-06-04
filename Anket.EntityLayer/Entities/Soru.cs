using ISUAnket.EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Entities
{
    public class Soru
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Soru Metni")]
        [StringLength(500)]
        public string SoruMetni { get; set; }

        [Display(Name ="Soru Tipi")]
        public SoruTipiEnum SoruTipi { get; set; }

        [Display(Name ="Seçenekler")]
        public string? SoruSecenekleri { get; set; }

        [Display(Name ="Oluşturan Kişi")]
        public int OlusturanKullaniciId { get; set; }
        public Kullanici OlusturanKullanici { get; set; }

        [Display(Name ="Oluşturma Tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        [Display(Name ="Düzenleyen Kişi")]
        public int? DuzenleyenKullaniciId { get; set; }
        public Kullanici? DuzenleyenKullanici { get; set; }

        [Display(Name ="Düzenlenme Tarihi")]
        public DateTime? DuzenlenmeTarihi { get; set; }

        [Display(Name ="Durum")]
        public bool AktifMi { get; set; } = true;
        public bool ZorunluMu { get; set; }
        public int? AnketId { get; set; }

        [Display(Name ="Anket")]
        public Anket? Anket { get; set; }
        public List<Cevap> Cevaplar { get; set; }
    }
}
