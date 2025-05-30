using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Entities
{
    public class Rol
    {
        public int Id { get; set; }

        [Display(Name ="Rol Adı")]
        public string RolAdi { get; set; }

        [Display(Name ="Durum")]
        public bool AktifMi { get; set; } = true;
        public List<Kullanici> Kullanicilar { get; set; }

    }
}
