using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.ViewModels
{
    public class SearchViewModel
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public bool already_friend { get; set; }
        public bool rqst_sent { get; set; }
        public bool rqst_received { get; set; }
        public string Photo { get; set; }
        public string User_Id { get; set; }
        public string search_data { get; set; }

    }
}
