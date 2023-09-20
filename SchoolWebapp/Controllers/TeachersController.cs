using IdentitySchoolWebap.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_webapp.Data;
using School_webapp.Models;

namespace School_webapp.Controllers
{
    public class TeachersController : Controller
    {
        private readonly School_webappContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> _roleManager;


        public TeachersController(School_webappContext context, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Teachers
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return _context.Teacher != null ?
                        View(await _context.Teacher.ToListAsync()) :
                        Problem("Entity set 'School_webappContext.Teacher'  is null.");
        }

        // GET: Teachers/Create

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("id,name,lastName,age,email,phone,address")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                var user = new User()
                {
                    Id = teacher.id.ToString() + "-T",
                    UserName = teacher.name.Substring(0, 1) + teacher.lastName.Substring(0, Math.Min(teacher.lastName.Length, 3)).Normalize() + teacher.id,
                    Email = "",
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                };
                var result = await _userManager.CreateAsync(user, user.UserName + "-Password");
                await _userManager.AddToRoleAsync(user, "Teacher");
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Teacher == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        [Authorize(Roles = "Admin")]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,lastName,age,email,phone,address")] Teacher teacher)
        {
            if (id != teacher.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Teacher == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.id == id);
            if (teacher == null)
            {
                return NotFound();
            }
            //delete user
            var user = await _userManager.FindByIdAsync(id.ToString() + "-U");
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]

        [Authorize(Roles = "Admin")]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Teacher == null)
            {
                return Problem("Entity set 'School_webappContext.Teacher'  is null.");
            }
            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher != null)
            {
                _context.Teacher.Remove(teacher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return (_context.Teacher?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
