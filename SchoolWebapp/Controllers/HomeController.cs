using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School_webapp.Data;
using School_webapp.Models;

namespace School_webapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly School_webappContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, School_webappContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;

        }
        /*        [Authorize(Roles = "Student, Admin, Teacher")]
        */
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
            var newEvent = new SchoolWebapp.Models.Event { };
            newEvent.Title = title;
            newEvent.DateStart = start;
            newEvent.UserId = _userManager.GetUserId(User);
            //insert into the database
            _context.Event.Add(newEvent);
            _context.SaveChanges();
            return View();
        }
    }
}