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

        [Display(Name ="TCKN")]
        [StringLength(11)]
        public string TCKN { get; set; }

        [Display(Name ="Ad")]
        [StringLength(100)]
        public string Ad { get; set; }

        [Display(Name ="Soyad")]
        [StringLength(100)]
        public string Soyad { get; set; }

        [Display(Name ="Kullanıcı Adı")]
        [StringLength(100)]
        public string KulaniciAdi { get; set; }

        //[StringLength(100)]
        //[EmailAddress]
        //public string Email { get; set; }

        [StringLength(200)]
        [Display(Name ="Şifre")]
        public string Sifre { get; set; }

        [Display(Name ="Oturum açık mı?")]
        public bool OturumAcikMi { get; set; } = false;

        [Display(Name ="Son Çıkış Tarihi")]
        public DateTime? SonCikisTarihi { get; set; }

        [Display(Name ="Durum")]
        public bool AktifMi { get; set; } = true;

        [Display(Name ="Rol")]
        public int? RolId { get; set; }
        public Rol? Rol { get; set; }
    }
}
