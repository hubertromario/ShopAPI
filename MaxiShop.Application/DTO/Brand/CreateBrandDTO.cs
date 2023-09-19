using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.DTO.Brand
{
    public class CreateBrandDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int year { get; set; }
    }
}
