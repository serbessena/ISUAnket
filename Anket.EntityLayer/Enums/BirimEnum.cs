using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Enums
{
    public enum BirimEnum
    {
        [Display(Name ="Merkez")]
        Merkez=1,
        [Display(Name ="Şube")]
        Sube=2 
        
    }
}
