using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Dtos.KullaniciDtos
{
    public class KullaniciEkleDto
    {
        [Required(ErrorMessage = "TC Kimlik Numarası alanı boş geçilemez.")]
        [Display(Name = "TC Kimlik No")]
        [StringLength(11, ErrorMessage = "TC Kimlik Numarası 11 karakterden oluşmalıdır.")]
        public string TCKN { get; set; }


        [Required(ErrorMessage = "Ad alanı boş geçilemez.")]
        [StringLength(100, ErrorMessage = "Ad 100 karakterden fazla olamaz")]
        [MinLength(3, ErrorMessage = "Ad alanı için minimum 3 karakter girilmesi gerekmektedir.")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad alanı boş geçilemez.")]
        [StringLength(100, ErrorMessage = "Soyad 100 karakterden fazla olamaz")]
        [MinLength(2, ErrorMessage = "Soyad alanı için minimum 3 karakter girilmesi gerekmektedir.")]
        public string Soyad { get; set; }


        [Required(ErrorMessage = "Kullanıcı Adı alanı boş geçilemez.")]
        [Display(Name = "Kullanıcı Adı")]
        [StringLength(100, ErrorMessage = "Kullanıcı Adı 100 karakterden fazla olamaz")]
        [MinLength(2, ErrorMessage = "Soyad alanı için minimum 3 karakter girilmesi gerekmektedir.")]
        public string KullaniciAdi { get; set; }


        [Required(ErrorMessage = "Şifre alanı boş geçilemez.")]
        [Display(Name = "Şifre")]
        [StringLength(200, ErrorMessage = "Şifre 200 karakterden fazla olamaz")]
        [MinLength(6, ErrorMessage = "Şifre için minimum 6 karakter girilmesi gerekmektedir.")]
        public string Sifre { get; set; }

        [Display(Name = "Rolü")]
        public int RolId { get; set; }

        // (Opsiyonel) Dropdown için roller
        public List<SelectListItem> Roller { get; set; }
    }
}
