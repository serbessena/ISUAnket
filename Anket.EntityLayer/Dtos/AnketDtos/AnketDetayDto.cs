using ISUAnket.EntityLayer.Entities;
using ISUAnket.EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Dtos.AnketDtos
{
    public class AnketDetayDto
    {
        public int Id { get; set; }

        [Display(Name = "Anket Adı")]
        public string Ad { get; set; }

        [Display(Name = "Bağlantı Linki")]
        public string? Link { get; set; }

        [Display(Name = "Başlangıç Tarihi")]
        public DateTime BaslangicTarihi { get; set; }

        [Display(Name = "Bitiş Tarihi")]
        public DateTime BitisTarihi { get; set; }

        [Display(Name = "Anket Durumu")]
        public AnketDurumuEnum AnketDurumu { get; set; }

        [Display(Name = "Oluşturan Kullanıcı")]
        public int OlusturanKullaniciId { get; set; }
        public Kullanici OlusturanKullanıcı { get; set; }

        [Display(Name = "Oluşturma Tarihi")]
        public DateTime OlusturmaTarihi { get; set; }

        [Display(Name = "Düzenleyen Kullanıcı")]
        public int? DuzenleyenKullaniciId { get; set; }
        public Kullanici? DuzenleyenKullanici { get; set; }

        [Display(Name = "Düzenlenme Tarihi")]
        public DateTime? DuzenlenmeTarihi { get; set; }

        [Display(Name = "Aktif mi ?")]
        public bool AktifMi { get; set; }
        public List<Soru> Sorular { get; set; }
    }
}
