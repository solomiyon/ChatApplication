using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.DAL.Entity
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Data { get; set; }
        public ChatType Type { get; set; }
        public ICollection<ChatUser> Participants { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
