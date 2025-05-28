using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Dtos.KullaniciDtos
{
    public class KullaniciListeDto
    {
        [Display(Name = "TC Kimlik No")]
        public string TCKN { get; set; }

        [Display(Name = "Ad")]
        public string Ad { get; set; }

        [Display(Name = "Soyad")]
        public string Soyad { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string KullaniciAdi { get; set; }

        [Display(Name = "Rol Adı")]
        public string RolAdi { get; set; }
    }
}
