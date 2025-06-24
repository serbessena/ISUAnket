using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Entities
{
    public class Kullanici
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="TCKN zorunludur!")]
        [Display(Name ="TCKN")]
        [StringLength(11)]
        [MaxLength(11,ErrorMessage ="Kimlik numaranız 11 hane olmalıdır!")]
        public string TCKN { get; set; }

        [Required(ErrorMessage ="Ad alanı zorunludur!")]
        [Display(Name ="Ad")]
        [StringLength(100)]
        [MaxLength(100,ErrorMessage ="Ad alanı en fazla 100 karakter uzunluğunda olmalıdır!")]
        [MinLength(3,ErrorMessage ="Ad en az 3 karakter uzunluğunda olmalıdır!")]
        public string Ad { get; set; }

        [Required(ErrorMessage ="Soyad alanı zorunludur!")]
        [Display(Name ="Soyad")]
        [StringLength(100)]
        [MaxLength(100,ErrorMessage ="Soyad alanı en fazla 100 karakter uzunluğunda olmalıdır!")]
        [MinLength(2,ErrorMessage ="Soyad en az 2 karakter uzunluğunda olmalıdır!")]
        public string Soyad { get; set; }

        [Required(ErrorMessage ="Kullanıcı adı zorunludur!")]
        [Display(Name ="Kullanıcı Adı")]
        [StringLength(100)]
        [MaxLength(100,ErrorMessage ="Kullanıcı adı en fazla 100 karakter uzunluğunda olmalıdır!")]
        public string KulaniciAdi { get; set; }

        [Required(ErrorMessage = "Eposta adresi zorunludur!")]
        [Display(Name = "Eposta")]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage ="Şifre zorunludur!")]
        [StringLength(200)]
        [Display(Name ="Şifre")]
        public string Sifre { get; set; }

        [Display(Name ="Oturum açık mı?")]
        public bool OturumAcikMi { get; set; } = false;

        [Display(Name ="Son Çıkış Tarihi")]
        public DateTime? SonCikisTarihi { get; set; }

        [Display(Name ="Aktif mi")]
        public bool AktifMi { get; set; } = true;

        [Display(Name ="Rol")]
        public int? RolId { get; set; }
        public Rol? Rol { get; set; }
    }
}
