using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.ViewModels
{
    public class ProfileView
    {
        [Required]
        [MaxLength(20)]
        public string First_Name { get; set; }
        [Required]
        [MaxLength(20)]
        public string Last_Name { get; set; }
        public IFormFile  Photo { get; set; }
    }
}
