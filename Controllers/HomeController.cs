using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LoginRegistration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoginRegistration.Controllers {
    public class HomeController : Controller {

        private MyContext _context { get; set; }

        public HomeController (MyContext context) {
            _context = context;
        }

        [HttpGet ("")]
        public IActionResult Index () {
            return View ();
        }

        [HttpPost ("register")]
        public IActionResult Register (User user) {
            if (ModelState.IsValid) {
                // If a User exists with provided email
                if (_context.Users.Any (u => u.Email == user.Email)) {
                    ModelState.AddModelError ("Email", "Email already in use!");
                    return View ("Index");
                } else {
                    PasswordHasher<User> Hasher = new PasswordHasher<User> ();
                    user.Password = Hasher.HashPassword (user, user.Password);
                    _context.Users.Add (user);
                    _context.SaveChanges ();
                    HttpContext.Session.SetInt32 ("userid", user.UserId);
                    //will need to turn this into LoginUser
                    // LoginUser toLogin = new LoginUser ();
                    // toLogin.LoginEmail = user.Email;
                    // toLogin.LoginPassword = user.Password;
                    return Redirect ("Home");
                }
            }
            return View ("Index");
        }
        // [HttpGet("NewUserLogin")]
        // public IActionResult NewUserLogin (User user) {

        //     return Redirect ("Home");
        // }

        [HttpPost ("login")]
        public IActionResult Login (LoginUser userLogin) {
            if (ModelState.IsValid) {
                var userInDb = _context.Users.FirstOrDefault (u => u.Email == userLogin.LoginEmail);
                if (userInDb == null) {
                    ModelState.AddModelError ("LoginEmail", "Invalid Email/Password");
                    return View ("Index");
                } else {
                    var hasher = new PasswordHasher<LoginUser> ();
                    var result = hasher.VerifyHashedPassword (userLogin, userInDb.Password, userLogin.LoginPassword);
                    if (result == 0) {
                        ModelState.AddModelError ("LoginEmail", "Invalid Email/Password");
                        return View ("Index");
                    } else {
                        //initialize session...?
                        HttpContext.Session.SetInt32 ("userid", userInDb.UserId);
                        return Redirect ("Home");
                    }
                }
            } else {
                return View ("Index");
            }
        }

        [HttpGet ("Home")]
        public IActionResult Home () {
            int? userid = HttpContext.Session.GetInt32 ("userid");
            if (userid == null) {
                return View ("Index");
            } else {
                return View (userid);
            }
        }
        [HttpGet("Logout")]
        public IActionResult Logout () {
            HttpContext.Session.Clear ();
            return RedirectToAction ("Index");
        }
    }
}