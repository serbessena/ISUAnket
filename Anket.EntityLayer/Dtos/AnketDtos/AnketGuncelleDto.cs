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
    public class AnketGuncelleDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı boş geçilemez.")]
        [StringLength(100, ErrorMessage = "Ad 100 karakterden fazla olamaz")]
        [MinLength(3, ErrorMessage = "Ad alanı için minimum 3 karakter girilmesi gerekmektedir.")]
        public string Ad { get; set; }

        [StringLength(2000, ErrorMessage = "Soyad 2000 karakterden fazla olamaz")]
        public string? Link { get; set; }

        [Required(ErrorMessage = "Başlangıç Tarihi boş geçilemez.")]

        public DateTime BaslangicTarihi { get; set; }

        [Required(ErrorMessage = "Bitiş Tarihi boş geçilemez.")]
        [Display(Name = "Bitiş Tarihi")]
        public DateTime BitisTarihi { get; set; }

        [Display(Name = "Anket Durumu")]
        public AnketDurumuEnum AnketDurumu { get; set; } = AnketDurumuEnum.Taslak;

        [Display(Name = "Oluşturan Kullanıcı")]
        public int OlusturanKullaniciId { get; set; }
        public Kullanici OlusturanKullanıcı { get; set; }

        [Display(Name = "Oluşturma Tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

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
