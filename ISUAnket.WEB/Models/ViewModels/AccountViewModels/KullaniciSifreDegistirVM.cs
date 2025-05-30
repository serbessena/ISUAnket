using System.ComponentModel.DataAnnotations;

namespace ISUAnket.WEB.Models.ViewModels.AccountViewModels
{
    public class KullaniciSifreDegistirVM
    {
        [Required]
        public string KullaniciAdi { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string MevcutSifre { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string YeniSifre { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("YeniSifre", ErrorMessage = "Yeni şifreler uyuşmuyor.")]
        public string YeniSifreTekrar { get; set; }
    }
}
