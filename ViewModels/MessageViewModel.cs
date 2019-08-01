using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialNetwork.Models;
namespace SocialNetwork.ViewModels
{
    public class MessageViewModel
    {

        public Profile SenderProfile { get; set; }
        public Profile ReceiverProfile { get; set; }
        public Message  Message { get; set; } 
        public List<Profile> Friend_List { get; set; }

    }
}
