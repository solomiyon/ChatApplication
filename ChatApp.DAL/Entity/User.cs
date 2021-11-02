using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ChatApp.DAL.Entity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImagePath { get; set; }
        public UserRole UserRole { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<ChatUser> Chats { get; set; }
    }
}
