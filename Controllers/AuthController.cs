using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using Blogsiotoh.Models;
using Blogsiotoh.Services;
using Blogsiotoh.Utilities;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blogsiotoh.Controllers
{
    public class AuthController : Controller
    {
        public readonly BlogContext db;
        private readonly IAuthServices authServices;

        
        public AuthController(BlogContext _db, IAuthServices _authServices)
        {
            db = _db;
            authServices = _authServices;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            ViewBag.status = null;
            ViewBag.message = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login login)
        {
            Response response = new Response();
            response.Status = 0;

            const string SessionId = "Id";
            const string SessionEmail = "Email";
            const string SessionFirstName = "FirstName";
            var obj = db.Users.Where(a => a.Email.Equals(login.Email)).FirstOrDefault();
            if (obj != null)
            {
                //check password
                var checkPassword = Crypto.VerifyHashedPassword(obj.Password, login.Password);
                if (checkPassword)
                {
                HttpContext.Session.SetString(SessionId, obj.Id.ToString());
                HttpContext.Session.SetString(SessionEmail, obj.Email);
                HttpContext.Session.SetString(SessionFirstName, obj.FirstName);
                response.Status = 1;
                response.Message = "Login Successful";
                return RedirectToAction("Index", "Home");
                }
                else
                {
                    response.Message = "Email or password is wrong";
                }
            }
            else
            {
                response.Message = "Login failed";
            }
            ViewBag.status = response.Status;
            ViewBag.message = response.Message;
            return View();
        }


        public IActionResult Register()
        {
            ViewBag.status = null;
            ViewBag.message = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(SignUp sign)
        {
            Response response = new Response();
            response = await authServices.Signup(sign);
            if(response.Status == 1) return RedirectToAction("Login", "Auth");
            ViewBag.status = response.Status;
            ViewBag.message = response.Message;
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
            //            return View();
        }

        public bool IfLogged()
        {
            if (HttpContext.Session.GetString("Id") == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

