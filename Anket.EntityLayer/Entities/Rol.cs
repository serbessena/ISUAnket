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

        [Required(ErrorMessage ="Rol adı zorunludur!")]
        [Display(Name ="Rol Adı")]
        [StringLength(50)]
        [MaxLength(50,ErrorMessage ="Rol adı en fazla 50 karakter uzunluğunda olmalıdır!")]
        [MinLength(5,ErrorMessage ="Rol adı en az 5 karakter uzunluğunda olmalıdır!")]
        public string RolAdi { get; set; }

        [Display(Name ="Aktif mi?")]
        public bool AktifMi { get; set; } = true;
        public List<Kullanici> Kullanicilar { get; set; }

        public List<MenuRol> MenuRoller { get; set; } = new();

    }
}
