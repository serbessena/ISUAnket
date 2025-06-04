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

        [Display(Name = "Anket Adı")]
        [StringLength(100)]
        public string Ad { get; set; }

        [Display(Name = "Anket Bağlantısı")]
        [StringLength(2000)]
        public string? Link { get; set; }

        [Display(Name ="Başlangıç Tarihi")]
        public DateTime BaslangicTarihi { get; set; }

        [Display(Name ="Bitiş Tarihi")]
        public DateTime BitisTarihi { get; set; }

        [Display(Name ="Anket Durumu")]
        public AnketDurumuEnum AnketDurumu { get; set; }

        [Display(Name ="Oluşturulma Tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        [Display(Name ="Oluşturan Kullanıcı")]
        public int OlusturanKullaniciId { get; set; }
        public Kullanici OlusturanKullanici { get; set; }

        [Display(Name ="Düzenleyen Kullanıcı")]
        public int? DuzenleyenKullaniciId { get; set; }
        public Kullanici? DuzenleyenKullanici { get; set; }

        [Display(Name ="Düzenlenme Tarihi")]
        public DateTime? DuzenlenmeTarihi { get; set; }

        [Display(Name ="Aktif mi?")]
        public bool AktifMi { get; set; }
        public List<Soru> Sorular { get; set; }
    }
}
