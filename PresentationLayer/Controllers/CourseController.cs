using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class CourseController : Controller
    {
        CourseManager _courseManager = new CourseManager(new EfCourseDal());
        public IActionResult Index(string SearchString)
        {
            using var c = new Context();
            var query = c.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(x =>
                x.Name.Contains(SearchString));
            }
            var results = query.ToList();
            return View(results);
        }
        [HttpGet]
        public IActionResult AddCourse() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCourse(Course course)
        {
            CourseValidator validationRules = new CourseValidator();
            ValidationResult results = validationRules.Validate(course);
            if (results.IsValid)
            {
                _courseManager.TAdd(course);
                return RedirectToAction("Index","Course");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName,item.ErrorMessage);
                }
            }
            return View();
        }
        public IActionResult DeleteCourse(int id)
        {
            var value = _courseManager.TGetById(id);
            _courseManager.TDelete(value);
            return RedirectToAction("Index","Course");
        }
        [HttpGet("/Course/EditCourse/{id}")]
        public IActionResult EditCourse(int id)
        {
            var value = _courseManager.TGetById(id);
            return View(value);
        }
        [HttpPost("/Course/EditCourse/{id}")]
        public IActionResult EditCourse(Course course)
        {
            CourseValidator validationRules = new CourseValidator();
            ValidationResult results = validationRules.Validate(course);
            if (results.IsValid)
            {
                _courseManager.TUpdate(course);
                return RedirectToAction("Index","Course");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName,item.ErrorMessage);
                }
            }
            return View();
        }
    }
}
