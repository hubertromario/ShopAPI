using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Input_Models
{
    public class Register
    {
        [Required]
        public string firstName {  get; set; }

        public string lastName { get; set; }

        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
