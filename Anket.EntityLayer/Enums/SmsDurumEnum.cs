using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Enums
{
    public enum SmsDurumEnum
    {
        [Display(Name ="Hata")]
        Hata=0,
        [Display(Name ="İletildi")]
        Iletildi=1,
        [Display(Name = "İletilmedi")]
        Iletilmedi = 2,

    }
}
