using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Enums
{
    public enum VeriTipiEnum
    {
        [Display(Name ="Metin")]
        Metin=0,
        [Display(Name ="Sayısal")]
        Sayisal=1,
        [Display(Name ="Tarih")]
        Tarih=2, 
        [Display(Name ="Email")]
        Email=3,
        [Display(Name = "Virgüllü Sayı")]
        Double = 4,
        [Display(Name = "Parasal Değer")]
        Decimal = 5
    }
}
