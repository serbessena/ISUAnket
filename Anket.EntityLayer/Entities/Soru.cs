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

        [StringLength(500)]
        public string SoruMetni { get; set; }
        public SoruTipiEnum? SoruTipi { get; set; }
        public string? SoruSecenekleri { get; set; }
        public int OlusturanKullaniciId { get; set; }
        public Kullanici OlusturanKullanici{ get; set; }
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public int? DuzenleyenKullaniciId{ get; set; }
        public Kullanici? DuzenleyenKullanici{ get; set; }
        public DateTime? DuzenlenmeTarihi{ get; set; }
        public bool AktifMi { get; set; }
        public bool? ZorunluMu { get; set; }
        public int? AnketId { get; set; }
        public Anket? Anket { get; set; }
        public List<Cevap> Cevaplar { get; set; }
    }
}
