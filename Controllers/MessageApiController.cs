using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Data;
using SocialNetwork.Models;
using SocialNetwork.ViewModels;
namespace SocialNetwork.Controllers
{
  
    public class MessageApiController : Controller
    {
        private ApplicationDbContext _context;
        
        public MessageApiController(ApplicationDbContext context)
        {
            _context = context;   

        }
        [HttpGet]
        public async Task<ActionResult> GetMessages(string id)
        {
            string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            SendMessageApiModel msg = new SendMessageApiModel();
            try
            {
                Message message = _context.Messages.First(a => a.Sender == id && a.Receiver == userid && a.seen == false);
                msg.Sender_Profile_Picture = _context.Profiles.First(a => a.User_Id == message.Sender).profile_Picture_Url;
                msg.Sender_First_Name = _context.Profiles.First(a => a.User_Id == message.Sender).First_Name;
                msg.Sender_Last_Name = _context.Profiles.First(a => a.User_Id == message.Sender).Last_Name;
                msg.Message = message.Body;
                await _context.SaveChangesAsync();
                message.seen = true;
               _context.Update(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception a)
            {
                
                msg = null;
            }
            return Json(msg);
        }
        public async Task<IActionResult> SendMessage(string id, string message)
        {
            
            Message msg = new Message();
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            msg.Sender = userid;
            msg.Receiver = id;
            msg.Body = message;
            await _context.Messages.AddAsync(msg);
            await _context.SaveChangesAsync();
            SendMessageApiModel msgmodel = new SendMessageApiModel();
            msgmodel.Sender_Profile_Picture = _context.Profiles.First(a => a.User_Id == userid).profile_Picture_Url;
            msgmodel.Sender_First_Name = _context.Profiles.First(a => a.User_Id == userid).First_Name;
            msgmodel.Sender_Last_Name = _context.Profiles.First(a => a.User_Id == userid).Last_Name;
            msgmodel.Message = message;
            return Json(msgmodel);
        }

    }
}