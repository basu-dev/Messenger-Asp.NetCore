using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Data;
using SocialNetwork.Models;
using SocialNetwork.ViewModels;
namespace SocialNetwork.Controllers
{
    public class MessageController : Controller
    {
        
        private ApplicationDbContext _context;
        string userid;
       public  List<MessageViewModel> messageViewModels=new List<MessageViewModel>();
        public MessageController(ApplicationDbContext context)
        {
            _context = context;
           

        }
        [Authorize]
        [HttpGet]
        public IActionResult Index(string id)
        {
            userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            ViewBag.userid = userid;
            List<Profile> frnreq_profile = new List<Profile>();
            List<Friend_Request> frnreq = _context.Friend_Requests.Where(a => a.Receiver == userid).ToList();
            foreach(Friend_Request fq in frnreq)
            {
                frnreq_profile.Add(_context.Profiles.First(a => a.User_Id == fq.Sender));
            }
            ViewBag.frnreq_list = frnreq_profile;
            List<Profile> frn_profile = new List<Profile>();
            List<Friend> friends = _context.Friends.Where(a => a.Sender == userid || a.Receiver == userid).ToList();
            
            foreach(Friend frn in friends)
            {
                string frn_id;
                if (frn.Sender == userid)
                {
                    frn_id = frn.Receiver;
                }
                else
                {
                    frn_id = frn.Sender;
                }
                frn_profile.Add(_context.Profiles.Where(a => a.User_Id == frn_id).First());

                
            }
            ViewBag.frn_list = frn_profile;
            try
            {
                Profile receiver = _context.Profiles.First(a => a.User_Id == id);
                ViewBag.receiver = receiver;
            }
            catch(Exception e)
            {
                ViewBag.receiver=null;
            }
           
            

            List<Message> my_message = _context.Messages.Where(a => a.Receiver == userid && a.Sender == id || a.Sender == userid && a.Receiver == id).OrderByDescending(a=>a.Id).Take(25).OrderBy(a=>a.Id).ToList();
           // return Json(my_message);
            foreach (Message msg in my_message)
            {
                MessageViewModel mvm = new MessageViewModel
                {
                    Message=msg,
                    SenderProfile=_context.Profiles.Where(a=>a.User_Id==msg.Sender).First(),
                    ReceiverProfile=_context.Profiles.Where(a=>a.User_Id==msg.Receiver).First(),

                };
                if (msg.Sender == id)
                {
                    msg.seen = true;
                }
                _context.SaveChanges();
               
                messageViewModels.Add(mvm);
            }
            ViewBag.receiver_id=id;
            
            return View(messageViewModels);


        }
        [HttpPost]
        public async Task<IActionResult> Index(string message,string id)
        {
            userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Message newmsg = new Message();
            newmsg.Body = message;
            newmsg.Sender = userid;
            newmsg.Receiver = id;
            await _context.Messages.AddAsync(newmsg);
            await _context.SaveChangesAsync();
            return LocalRedirect("/Message/Index/"+id);
        }
        
        
    }
}