using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.DAL.Entity
{
    public class ChatUser
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int ChatId { get; set; }

        public Chat Chat { get; set; }
    }
}
