using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _301171014deguzman_301128209alvarado_Lab3.Models;
using Microsoft.AspNetCore.Http;

namespace _301171014deguzman_301128209alvarado_Lab3.Controllers
{
    public class UsersController : Controller
    {
        private readonly MOVIEContext _context;

        public UsersController(MOVIEContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.MovieUsers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.MovieUsers
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        /*        // GET: Customers/Create
                public IActionResult Create()
                {
                    return View();
                }

                // POST: Customers/Create
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create([Bind("Username,Password,FirstName,LastName,Email")] Customer customer)
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(customer);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    return View(customer);
                }*/

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.MovieUsers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,Password,FirstName,LastName,Email")] User customer)
        {
            if (id != customer.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.UserId))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.MovieUsers
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var customer = await _context.MovieUsers.FindAsync(id);
            _context.MovieUsers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(string id)
        {
            return _context.MovieUsers.Any(e => e.UserId == id);
        }

        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("UserId,Password")] LoginModel objLoginModel)
        {

            if (ModelState.IsValid)
            {
                if (_context.MovieUsers.Where(e => e.UserId == objLoginModel.UserId && e.Password == objLoginModel.Password).FirstOrDefault() == null)
                {
                    ModelState.AddModelError("Error", "Invalid Username or Password");
                }
                else
                {
                    HttpContext.Session.SetString("LoggedInUser", objLoginModel.UserId);
                    //return RedirectToAction("Index", "Movies", objLoginModel);
                    return RedirectToAction("Index", "Movies");
                }
            }
            return View();
        }


        // GET: Users/Register
        public IActionResult Register()
        {
            return View();
        }


        // POST: Customers/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserId,Password,FirstName,LastName,Email")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();

                // loggedinuser for partial
                //ViewData["LoggedInUser"] = customer.Username;
                //return View();
                HttpContext.Session.SetString("LoggedInUser", user.UserId);
                return RedirectToAction("Index", "Home");

            }
            return RedirectToAction("Index", "Home");
        }


        // GET: Users/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Restricted
        public IActionResult Restricted()
        {
            return View();
        }
    }
}
