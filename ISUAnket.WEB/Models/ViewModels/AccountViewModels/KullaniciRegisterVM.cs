using System.ComponentModel.DataAnnotations;

namespace ISUAnket.WEB.Models.ViewModels.AccountViewModels
{
    public class KullaniciRegisterVM
    {
        [Required]
        [StringLength(11)]
        public string TCKN { get; set; }

        [Required]
        public string Ad { get; set; }

        [Required]
        public string Soyad { get; set; }

        [Required]
        public string KullaniciAdi { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }

        [Required]
        [Compare("Sifre", ErrorMessage = "Şifreler uyuşmuyor")]
        [DataType(DataType.Password)]
        public string SifreTekrar { get; set; }
    }
}
