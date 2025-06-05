using System.ComponentModel.DataAnnotations;

namespace ISUAnket.WEB.Models.ViewModels.AccountViewModels
{
    public class KullaniciSifremiUnuttumVM
    {
        [Required(ErrorMessage ="Tc kimlik numarası zorunludur")]
        [Display(Name ="TCKN")]
        [StringLength(11)]
        [MaxLength(11,ErrorMessage ="Kimlik numaranız 11 haneli olmalıdır!")]
        [MinLength(11,ErrorMessage ="Lütfen 11 haneli kimlik numaranızı giriniz!")]
        public string TCKN { get; set; }

        [Required(ErrorMessage ="Kullanıcı adı zorunludur")]
        [Display(Name ="Kullanıcı adı")]
        public string KullaniciAdi { get; set; }
    }
}
