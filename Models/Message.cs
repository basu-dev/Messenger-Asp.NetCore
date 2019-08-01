using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Receiver { get; set; }
        public string Sender { get; set; }
        public string Body { get; set; }
        public DateTime Sent_At { get; set; }
       
        public bool seen { get; set; }
    }
}
