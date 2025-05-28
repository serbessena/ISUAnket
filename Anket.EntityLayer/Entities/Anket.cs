using ISUAnket.EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Entities
{
    public class Anket
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Ad { get; set; }
        [StringLength(2000)]
        public string? Link { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public AnketDurumuEnum AnketDurumu { get; set; }
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public int OlusturanKullaniciId { get; set; }
        public Kullanici OlusturanKullanici { get; set; }
        public int? DuzenleyenKullaniciId { get; set; }
        public Kullanici? DuzenleyenKullanici { get; set; }
        public DateTime? DuzenlenmeTarihi { get; set; }
        public bool AktifMi { get; set; }
        public List<Soru> Sorular { get; set; }
    }
}
