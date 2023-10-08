using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;
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

        [HttpGet("/Student/EditStudent/{id}")]
        public IActionResult EditStudent(int id)
        {
            var value = _studentManager.TGetById(id);
            return View(value);
        }
        [HttpPost("/Student/EditStudent/{id}")]
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
        public IActionResult StudentDetails(int id)
        {
            using var c = new Context();
            var studentsAndExams = c.ExamResults
                .Include(x => x.Student)
                .Include(x => x.Course)
                .Where(x => x.Student.Id == id)
                .ToList();
            return View(studentsAndExams);
        }

        [HttpGet("/Student/StudentExamAverages/{studentNumber}/")]
        public IActionResult StudentExamAverages(int studentNumber)
        {
            using var c = new Context();
            var courses = c.Courses.ToList();

            var model = new List<Tuple<string, double>>();

            foreach (var course in courses)
            {
                int numberOfScores = c.ExamResults
            .Where(er => er.Student.Id == studentNumber && er.Course.Id == course.Id)
            .Count();
                if (numberOfScores >= 3)
                {
                    double average = c.ExamResults
                        .Where(er => er.Student.Id == studentNumber && er.Course.Id == course.Id)
                        .Average(er => er.Score);

                    model.Add(new Tuple<string, double>(course.Name, average));
                }
            }
            return View(model);
        }

        [HttpGet("/Student/StudentScoreAdd/{studentNumber}/")]
        public IActionResult StudentScoreAdd(int studentNumber)
        {
            using var c = new Context();
            var courses = c.Courses.ToList();

            SelectList courseList = new SelectList(courses, "Id", "Name");

            ViewBag.CourseList = courseList;

            return View();
        }


        [HttpPost("/Student/StudentScoreAdd/{studentNumber}/")]
        public IActionResult StudentScoreAdd(int studentNumber, ExamResultViewModel model)
        {
            using var c = new Context();
            model.StudentId = studentNumber;


            var newExamResult = new ExamResult
            {
                StudentId = model.StudentId,
                CourseId = model.CourseId,
                Score = model.Score
            };

            c.ExamResults.Add(newExamResult);
            c.SaveChanges();

            return RedirectToAction("Index", "Student");
        }

    }
}
