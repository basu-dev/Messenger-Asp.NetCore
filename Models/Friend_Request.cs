using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Models
{
    public class Friend_Request
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }

    }
}
