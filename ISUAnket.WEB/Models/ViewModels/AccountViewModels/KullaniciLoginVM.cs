using System.ComponentModel.DataAnnotations;

namespace ISUAnket.WEB.Models.ViewModels.AccountViewModels
{
    public class KullaniciLoginVM
    {
        [Required(ErrorMessage ="Kullanıcı adı zorunludur!")]
        [Display(Name ="Kullanıcı Adı")]
        [MaxLength(50,ErrorMessage ="Kullanıcı adı en fazla 50 karakter uzunluğunda olmalıdır")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage ="Şifre zorunludur!")]
        [Display(Name ="Şifre")]
        public string Sifre { get; set; }
    }
}
