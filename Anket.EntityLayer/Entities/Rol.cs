using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Entities
{
    public class Rol
    {
        public int Id { get; set; }
        public string RolAdi { get; set; }
        public bool AktifMi { get; set; } = true;
        public List<Kullanici> Kullanicilar { get; set; }

    }
}
