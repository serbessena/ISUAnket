using System.ComponentModel.DataAnnotations;

namespace ISUAnket.WEB.Models.ViewModels.AccountViewModels
{
    public class KullaniciSifremiUnuttumVM
    {
        [Required]
        [StringLength(11)]
        public string TCKN { get; set; }

        [Required]
        public string KullaniciAdi { get; set; }
    }
}
