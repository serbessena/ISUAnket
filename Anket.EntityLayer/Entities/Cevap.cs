using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Entities
{
    public class Cevap
    {
        [Key]
        public int Id { get; set; }
        //public string IpAdress { get; set; }
        public int Birim { get; set; }
        [StringLength(600)]
        public string VerilenCevap { get; set; }
        public DateTime CevapTarihi { get; set; } = DateTime.Now;
        public int SoruId { get; set; }
        public Soru Soru { get; set; }
    }
}
