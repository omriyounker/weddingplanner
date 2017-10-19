using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using wedding.Models;
using System.Linq;

namespace wedding.Controllers
{
    public class UserController : Controller
    {
        private WeddingContext _context;

        public UserController(WeddingContext context)
        {
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("userid");
            return View("Index");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string Email, string Password)
        {   
            User account = _context.Users.SingleOrDefault(x => (x.Email == Email));
            if(account.Password == Password){
                HttpContext.Session.SetInt32("userid", account.UserId);
                return RedirectToAction("Dashboard");
            }
            ViewBag.Error = "Email or Password is incorrect";
            return View("Index");
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register(User user, string Password2){
            Console.WriteLine("GOT INTO REGISTER!");
             User account = _context.Users.SingleOrDefault(x => (x.Email == user.Email));
             if (account != null){
                 ViewBag.Error = "Account already exists for that email account";
                 return RedirectToAction("Index");
             }
            if(user.Password != Password2){
                ViewBag.Error = "Password and Password Verification must match";
                return RedirectToAction("Index");
            }
            if(ModelState.IsValid){
                _context.Users.Add(user);
                
                _context.SaveChanges();
                HttpContext.Session.SetInt32("userid", user.UserId);
                return RedirectToAction("Dashboard");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {   
            int? uid = HttpContext.Session.GetInt32("userid");
            List<User> users = _context.Users.Include(uno => uno.Going).ToList();
            User account = users.SingleOrDefault(x => (x.UserId == uid));
            List<Wedding> totallist = _context.Weddings.Include(one => one.Coming).ToList();
            
            ViewBag.userinfo = account;
            ViewBag.list = totallist;
            ViewBag.uid = (int)uid;

            return View("dashboard");
        }

        [HttpGet]
        [Route("new")]
        public IActionResult New(){
            return View();
        }
        [HttpPost]
        [Route("create")]
        public IActionResult Create(Wedding input){
            int? uid = HttpContext.Session.GetInt32("userid");
            input.Creator = (int)uid;
            _context.Weddings.Add(input);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [Route("leave/{num}")]
        public IActionResult Leave(int num){
            int? uid = HttpContext.Session.GetInt32("userid");
            
            Rsvp removing = _context.RSVPs.SingleOrDefault(rs => rs.WeddingId == num && rs.UserId == uid);
            _context.RSVPs.Remove(removing);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [Route("join/{num}")]
        public IActionResult Join(int num){
            int? uid = HttpContext.Session.GetInt32("userid");
            Rsvp newone = new Rsvp();
            newone.UserId = (int)uid;
            newone.WeddingId = num;
            _context.RSVPs.Add(newone);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [Route("delete/{num}")]
        public IActionResult Delete(int num){
            List<Wedding> list = _context.Weddings.Include(x => x.Coming).ToList();
            Wedding todelete = new Wedding();
            foreach(Wedding item in list){
                if(item.WeddingId == num){todelete = item;}
            }
            foreach(Rsvp stuff in todelete.Coming){
                _context.RSVPs.Remove(stuff);
            }
            _context.SaveChanges();
            _context.Weddings.Remove(todelete);
            _context.SaveChanges();
            
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("wedding/{num}")]
        public IActionResult Wedding(int num){
            if(unchecked( num != (int)num )){
                return RedirectToAction("Dashboard");
            }
            List<Wedding> list = _context.Weddings.Include(x => x.Coming).ThenInclude(s => s.User).ToList();
            Wedding touse = new Wedding();
            foreach(Wedding item in list){
                if(item.WeddingId == num){touse = item;}
            }
            ViewBag.Wed = touse;

            return View("One");
        }



    }
}
