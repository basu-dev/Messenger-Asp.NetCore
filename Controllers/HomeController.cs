using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    public class HomeController : Controller
    {
        private SignInManager<IdentityUser> _manager;
        private UserManager<IdentityUser> _usermanager;

        public HomeController(SignInManager<IdentityUser>manager,UserManager<IdentityUser>usermanager)
        {
            _manager = manager;
            _usermanager = usermanager;
                
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult LogOut()
        {
            _manager.SignOutAsync();
              return LocalRedirect("/");
        }
        public async Task< IActionResult> getuser()
        {
            string username = "santoshacharya2468@gmail.com";
            string password = "Manakamana123@";
            var user = new IdentityUser
            {
                Email = username,
                UserName = username,
            };
         
            var result = await _manager.PasswordSignInAsync(username, password, true,false);
            if (result.Succeeded)
            {
                return Content(result.ToString());
            }
            else
            {
                return Content("failed");
            }
            
        }
    }
}
