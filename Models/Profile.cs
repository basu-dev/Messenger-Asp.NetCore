using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string First_Name { get; set; } 
        public string Last_Name { get; set; }
        public string profile_Picture_Url { get; set; }
        public string User_Id { get; set; }
    }
}
