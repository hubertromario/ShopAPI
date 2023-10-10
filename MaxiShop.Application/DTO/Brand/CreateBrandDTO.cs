using FluentValidation;
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
        public string Name { get; set; }
        public int year { get; set; }
    }

    public class CreateBrandDTOValidator : AbstractValidator<CreateBrandDTO>
    {
        public CreateBrandDTOValidator() {
        

            RuleFor(x=>x.Name).NotNull().NotEmpty();
            RuleFor(x => x.year).InclusiveBetween(1920, DateTime.UtcNow.Year);
        }
    }
}
