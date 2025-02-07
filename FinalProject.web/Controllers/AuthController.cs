using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FinalProject.BLL;
using FinalProject.Models;

namespace FinalProject.web.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserService _userService;

        public AuthController()
        {
            _userService = new UserService();
        }

        // Show Register Page
        public ActionResult Register()
        {
            return View();
        }

        // Handle User Registration
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                bool isRegistered = _userService.RegisterUser(user);
                if (isRegistered)
                {
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError("", "Registration failed!");
            }
            return View();
        }

        // Show Login Page
        public ActionResult Login()
        {
            return View();
        }

        // Handle User Login
        [HttpPost]
        public ActionResult Login(User user)
        {
            var authenticatedUser = _userService.AuthenticateUser(user.Username, user.Password);
            if (authenticatedUser != null)
            {
                FormsAuthentication.SetAuthCookie(authenticatedUser.Username, false);
                Session["Username"] = authenticatedUser.Username;
                return RedirectToAction("Index", "DynamicTemplate");
            }
            ModelState.AddModelError("", "Invalid credentials!");
            return View();
        }

        // Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}