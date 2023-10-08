using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PresentationLayer.Controllers
{
    public class StudentController : Controller
    {
        StudentManager _studentManager = new StudentManager(new EfStudentDal());

        public IActionResult Index(string SearchString)
        {
            using var c = new Context();
            var query = c.Students.AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(x =>
                x.Full_Name.Contains(SearchString) ||
                x.Number.Contains(SearchString) ||
                x.Email.Contains(SearchString) ||
                x.Gsm_Number.Contains(SearchString));
            }
            var results = query.ToList();
            return View(results);
        }
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            StudentValidator validationRules = new StudentValidator();
            ValidationResult results = validationRules.Validate(student);
            if (results.IsValid)
            {
                _studentManager.TAdd(student);
                return RedirectToAction("Index", "Student");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }
        public IActionResult DeleteStudent(int id)
        {
            var value = _studentManager.TGetById(id);
            _studentManager.TDelete(value);
            return RedirectToAction("Index", "Student");
        }

        [HttpGet]
        public IActionResult EditStudent(int id)
        {
            var value = _studentManager.TGetById(id);
            return View(value);
        }
        [HttpPost]
        public IActionResult EditStudent(Student student)
        {
            StudentValidator validationRules = new StudentValidator();
            ValidationResult results = validationRules.Validate(student);
            if (results.IsValid)
            {
                _studentManager.TUpdate(student);
                return RedirectToAction("Index", "Student");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }
        //[HttpGet]
        //public IActionResult StudentDetails(int id)
        //{
        //    using var c = new Context();
        //    var customersAndOrders = c.ExamResults
        //        .Include(x => x.Student)
        //        .Include(x => x.Course)
        //        .Where(x => x.Student.Id == id)
        //        .ToList();
        //    return View(customersAndOrders);
        //}
        public IActionResult StudentDetails(int id)
        {
            using var c = new Context();
            var customersAndOrders = c.ExamResults
                .Include(x => x.Student)
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (customersAndOrders != null)
            {
                int nonNullScore = customersAndOrders.
            }
            return View(customersAndOrders);
        }
    }
}
