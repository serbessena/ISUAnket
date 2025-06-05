using System.ComponentModel.DataAnnotations;

namespace ISUAnket.WEB.Models.ViewModels.AccountViewModels
{
    public class KullaniciSifreDegistirVM
    {
        [Required]
        [Display(Name ="Kullanıcı adı")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage ="Mevcut şifre zorunludur!")]
        [Display(Name ="Mevcut şifre")]
        [DataType(DataType.Password)]
        public string MevcutSifre { get; set; }

        [Required(ErrorMessage ="Yeni Şifre")]
        [Display(Name ="Yeni Şifre")]
        [DataType(DataType.Password)]
        public string YeniSifre { get; set; }

        [Required(ErrorMessage ="Yeni Şifre alanı zorunludur!")]
        [Display(Name ="Yeni Şifre")]
        [DataType(DataType.Password)]
        [Compare("YeniSifre", ErrorMessage = "Yeni şifreler uyuşmuyor.")]
        public string YeniSifreTekrar { get; set; }
    }
}
