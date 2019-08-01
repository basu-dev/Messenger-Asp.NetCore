using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Models;
using SocialNetwork.ViewModels;

namespace SocialNetwork.Controllers
{
    public class ProfileController : Controller
    {
       
    
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;

        public ProfileController(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public IActionResult Index()
        {
           return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(ProfileView pp)
        {
            if (ModelState.IsValid)
            {
                if (pp.Photo != null)
                {
                    string filepath = Path.Combine(_env.WebRootPath, "images", "Profile_Picture", pp.Photo.FileName);
                    FileStream fs = new FileStream(filepath,FileMode.Create);
                    await pp.Photo.CopyToAsync(fs);
                    string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    Profile newprofile = _context.Profiles.First(a => a.User_Id == UserId);
                    newprofile.First_Name = pp.First_Name;
                    newprofile.Last_Name = pp.Last_Name;
                    newprofile.profile_Picture_Url = "/images/Profile_Picture/" + pp.Photo.FileName;
                    _context.Update(newprofile);
                    _context.SaveChanges();
                    return Redirect("Profile");

                    
                }
                else
                {
                    return View();
                }

            }
            else
            {
                return View();
            } 
        }
       
        public IActionResult Search(string username)
        {
         
            string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Profile> searches = _context.Profiles.Where(a => a.First_Name.Contains(username) || a.Last_Name.Contains(username)).ToList();
            List<SearchViewModel> search_results = new List<SearchViewModel>();
            foreach(Profile sr in searches)
            {
                SearchViewModel search_result = new SearchViewModel();
                search_result.First_Name = sr.First_Name;
                search_result.Last_Name = sr.Last_Name;
                search_result.Photo = sr.profile_Picture_Url;
                search_result.User_Id = sr.User_Id;
                search_result.search_data = username;
                Friend is_friend = new Friend();
                try
                {
                    is_friend = _context.Friends.First(a => a.Sender == sr.User_Id && a.Receiver == userid || a.Receiver == sr.User_Id && a.Sender == userid);
                    search_result.already_friend = true;
                    
                }
                catch(Exception e)
                {
                    search_result.already_friend = false;
                    try
                    {
                        Friend_Request is_rqst_sent = _context.Friend_Requests.First(a => a.Sender == userid && a.Receiver==sr.User_Id);
                       search_result.rqst_sent = true;
                    }
                    catch (Exception b)
                    {
                        search_result.rqst_sent = false;
                        try
                        {
                            Friend_Request is_rqst_received = _context.Friend_Requests.First(a => a.Receiver == userid && a.Sender==sr.User_Id);
                            search_result.rqst_received = true;
                        }
                        catch (Exception c)
                        {
                            search_result.rqst_received = false; 

                        }
                    }
                }
                search_results.Add(search_result);
            }
            ViewBag.userid = userid;
            return View(search_results);
        }
        [HttpPost]
        public async Task<IActionResult> Add_Friend(string id,string data)
        {
            string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Friend_Request fr = new Friend_Request();
            try
            {

                fr = _context.Friend_Requests.First(a => a.Sender == userid && a.Receiver == id || a.Sender == id && a.Receiver == userid);
                return Redirect(data);
            }
            catch (Exception e)
            {
                try
                {
                    Friend frn = _context.Friends.First(a => a.Sender == id && a.Receiver == userid || a.Sender == userid && a.Receiver == id);
                    return LocalRedirect(data);
                }
                catch
                {
                    Friend_Request nreq = new Friend_Request
                    {
                        Receiver = id,
                        Sender = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    };
                    await _context.Friend_Requests.AddAsync(nreq);
                    await _context.SaveChangesAsync();
                    return LocalRedirect(data);
                }


            }      

        }
        [HttpPost]
        public IActionResult Accept_Request(string id,string data)
        {
            string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                Friend_Request existing_req = _context.Friend_Requests.First(a => a.Receiver == userid && a.Sender == id);

                Friend newfrn = new Friend
                {
                    Sender = id,
                    Receiver = userid,

                };
                _context.Friends.Add(newfrn);
                _context.Friend_Requests.Remove(existing_req);
                _context.SaveChanges();
               
                return Redirect(data);
            }

            catch (Exception e)
            {
                return Content("what");
            }

           
        }
        [HttpPost]
        public IActionResult Deny_Request(string id,string data)
        {
            string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try { 
            Friend_Request existing_req = _context.Friend_Requests.First(a => a.Receiver == userid && a.Sender == id);
           
                _context.Friend_Requests.Remove(existing_req);
                _context.SaveChanges();
                return Redirect(data);

            }
            catch(Exception e)
            {

                return NotFound();
            }
        }
        [HttpPost]
        public IActionResult Cancel_Request(string id,string data)
        {
            string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                Friend_Request already_sent = _context.Friend_Requests.First(a => a.Sender == userid &&a.Receiver==id);
                _context.Friend_Requests.Remove(already_sent);
                _context.SaveChanges();
                return LocalRedirect(data);
            }
            catch(Exception e)
            {
                return LocalRedirect(data);
            }


        }
        [HttpPost]
        public IActionResult Unfriend(string id,string data)
        {
            string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                Friend frn = _context.Friends.First(a => a.Sender == userid && a.Receiver == id || a.Sender == id && a.Receiver == userid);
                _context.Friends.Remove(frn);
                _context.SaveChanges();
                return LocalRedirect(data);
        
            }
            catch (Exception e)
            {
                return LocalRedirect(data);
            }
        }

    }
}
