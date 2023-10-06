using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            RuleFor(x=>x.Full_Name).NotEmpty().WithMessage("Fill the blank with your fullname");
            RuleFor(x=>x.Number).NotEmpty().WithMessage("Fill the blank with your number");
            RuleFor(x=>x.Email).NotEmpty().WithMessage("Fill the blank with your email");
            RuleFor(x=>x.Gsm_Number).NotEmpty().WithMessage("Fill the blank with your gsmnumber");

        }
    }
}
