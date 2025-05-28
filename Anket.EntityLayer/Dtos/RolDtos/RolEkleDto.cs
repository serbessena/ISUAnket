using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.EntityLayer.Dtos.RolDtos
{
    public class RolEkleDto
    {
        [Required(ErrorMessage = "Rol adı boş geçilemez.")]
        [Display(Name = "Rol Adı")]
        public string RolAdi { get; set; }
    }
}
