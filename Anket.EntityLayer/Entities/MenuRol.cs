using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Entities
{
    public class MenuRol
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Menü Adı")]
        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        [Display(Name ="Rol Adı")]
        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}
