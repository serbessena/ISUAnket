using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Entities
{
    public class Kullanici
    {

        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Ad { get; set; }

        [StringLength(100)]
        public string Soyad { get; set; }

        [StringLength(100)]
        public string KulaniciAdi { get; set; }

        [StringLength(11)]
        public string TCKN { get; set; }

        [StringLength(200)]
        public string Sifre { get; set; }

        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}
