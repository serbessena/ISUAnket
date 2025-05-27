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
        public string Ad { get; set; }
        public string Soyad { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string KulaniciAdi { get; set; }

        [Display(Name = "Rolü")]
        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}
