using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School_webapp.Data;
using School_webapp.Models;
using SchoolWebapp.Models;
using System.Runtime.Versioning;
using System.Text.Json;

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
        [Authorize(Roles = "Student, Admin, Teacher")]
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var events = _context.Event.Where(e => e.UserId == userId).ToList();
            
            //serialize the events into a json string
            var ModifiecEvents = events.Select(e => new
            {
                title = e.Title,
                start = e.Start
            }).ToList();
            var jsonEvents = JsonSerializer.Serialize(ModifiecEvents);

            //viewbag
            ViewBag.Events = jsonEvents;
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
            newEvent.Start = start;
            newEvent.UserId = _userManager.GetUserId(User);
            //insert into the database
            _context.Event.Add(newEvent);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}