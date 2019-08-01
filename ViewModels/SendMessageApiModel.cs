using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SocialNetwork.Models;

namespace SocialNetwork.ViewModels
{
    public class SendMessageApiModel
    {
        [Required]
        public string  Sender_Profile_Picture { get; set; }
        [Required]
        public string Sender_First_Name { get; set; }
        public string Sender_Last_Name { get; set; }
        
        [Required]
        public string Message { get; set; }
    }
}
