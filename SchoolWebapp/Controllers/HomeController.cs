using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School_webapp.Data;
using School_webapp.Models;
using Event = SchoolWebapp.Models.Event;

namespace School_webapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly School_webappContext _context;

        public HomeController(ILogger<HomeController> logger, School_webappContext context)
        {
            _logger = logger;
            _context = context;
        }
        [Authorize(Roles = "Student, Admin, Teacher")]

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]

        public IActionResult AddEvent()
        {
            //read the data from the modal
            var title = Request.Form["title"];
            DateTime start = DateTime.Parse(Request.Form["start"]);
            //insert into a class
            var newEvent = new Event
            {
                Title = title,
                Start = start
            };
            //insert into the database
            _context.Event.Add(newEvent);
            _context.SaveChanges();
            return View();
        }
    }
}