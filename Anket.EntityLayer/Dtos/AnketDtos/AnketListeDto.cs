using ISUAnket.EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Dtos.AnketDtos
{
    public class AnketListeDto
    {
        public int Id { get; set; }

        [Display(Name = "Anket Adı")]
        public string Ad { get; set; }

        [Display(Name = "Başlangıç Tarihi")]
        public DateTime BaslangicTarihi { get; set; }

        [Display(Name = "Bitiş Tarihi")]
        public DateTime BitisTarihi { get; set; }

        [Display(Name = "Anket Durumu")]
        public AnketDurumuEnum AnketDurumu { get; set; }

        [Display(Name = "Aktif mi ?")]
        public bool AktifMi { get; set; }
    }
}
