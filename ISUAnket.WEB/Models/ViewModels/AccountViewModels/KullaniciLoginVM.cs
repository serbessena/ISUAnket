using System.ComponentModel.DataAnnotations;

namespace ISUAnket.WEB.Models.ViewModels.AccountViewModels
{
    public class KullaniciLoginVM
    {
        [Required]
        public string KullaniciAdi { get; set; }

        [Required]
        public string Sifre { get; set; }
    }
}
