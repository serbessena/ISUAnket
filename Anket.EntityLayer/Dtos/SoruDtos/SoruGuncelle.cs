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
    public class SoruGuncelle
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Soru metni boş geçilemez.")]
        [StringLength(500, ErrorMessage = "Soru metni 500 karakterden fazla olamaz")]
        [MinLength(10, ErrorMessage = "Soru metni için minimum 10 karakter girilmesi gerekmektedir.")]
        [Display(Name = "Soru Metni")]
        public string SoruMetni { get; set; }

        [Required(ErrorMessage = "Soru tipi boş geçilemez.")]
        [Display(Name = "Soru Tipi")]
        public SoruTipiEnum SoruTipi { get; set; }

        [Display(Name = "Soruya ait cevaplar")]
        public string? SoruSecenekleri { get; set; }

        [Display(Name = "Düzenleyen Kullanıcı")]
        public int? DuzenleyenKullaniciId { get; set; }

        [Display(Name = "Düzenleme Tarihi")]
        public DateTime? DuzenlenmeTarihi { get; set; }
        [Display(Name = "Aktif mi ?")]
        public bool AktifMi { get; set; }

        [Display(Name = " Zorunlu mu ?")]
        public bool? ZorunluMu { get; set; }

        [Display(Name = "Anket Adı")]
        public int? AnketId { get; set; }
    }
}
