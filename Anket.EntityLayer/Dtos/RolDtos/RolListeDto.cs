using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Dtos.RolDtos
{
    public class RolListeDto
    {
        public int Id { get; set; }

        [Display(Name = "Rol Adı")]
        public string RolAdi { get; set; }

        public string KullaniciAdi { get; set; }
        public string KullaniciSoyadi { get; set; }

        [Display(Name = "Kullanıcı")]
        public List<Kullanici> Kullanicilar { get; set; }
    }
}
