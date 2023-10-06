using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class CourseValidator : AbstractValidator<Course>
    {
        public CourseValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Fill the blank with the course name");
        }
    }
}
