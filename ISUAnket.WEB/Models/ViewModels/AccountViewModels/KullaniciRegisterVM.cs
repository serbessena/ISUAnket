using System.ComponentModel.DataAnnotations;

namespace ISUAnket.WEB.Models.ViewModels.AccountViewModels
{
    public class KullaniciRegisterVM
    {
        [Required]
        [StringLength(11)]
        [Display(Name ="TCKN")]
        public string TCKN { get; set; }

        [Required(ErrorMessage ="Ad alanı zorunludur")]
        [Display(Name ="Ad")]
        [MaxLength(50,ErrorMessage ="Ad alanı en fazla 50 karakter uzunluğunda olmalıdır!")]
        public string Ad { get; set; }

        [Required]
        [Display(Name ="Soyad")]
        [MaxLength(50,ErrorMessage ="Soyad alanı en fazla 50 karakter uzunluğunda olmalıdır!")]
        public string Soyad { get; set; }

        [Required]
        [Display(Name ="Kullanıcı Adı")]
        [MaxLength(50,ErrorMessage ="Kullanıcı adı en fazla 50 karakter uzunluğunda olmalıdır!")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage ="Şifre zorunludur!")]
        [Display(Name ="Şifre")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }

        [Required]
        [Compare("Sifre", ErrorMessage = "Şifreler uyuşmuyor")]
        [DataType(DataType.Password)]
        public string SifreTekrar { get; set; }
    }
}
