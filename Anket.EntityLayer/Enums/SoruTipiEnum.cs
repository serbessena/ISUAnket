using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Enums
{
    public enum SoruTipiEnum
    {
        [Display(Name ="Tek Satır Metin")]
        TekSatırMetin=0,
        [Display(Name ="Çok Satır Metin")]
        ÇokSatırMetin=1,
        [Display(Name ="Çoktan Seçmeli")]
        ÇoktanSeçmeli=2, //checkbox
        [Display(Name ="Tek Seçimli")]
        TekSeçimli=3 //radiobutton
    }
}
