using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Controllers
{
    public class DashboardController : Controller
    {
        private readonly Context _context;

        public DashboardController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //var customersAndOrders = _context.ExamResults
            //    .Include(x => x.Student)
            //    .Include(x => x.Course)
            //    .ToList();
            //return View(customersAndOrders);
            return View();
        }
    }
}
