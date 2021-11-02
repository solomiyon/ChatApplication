using ChatApp.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.BLL.DTO
{
    public class ChatDTO
    {
        public string Name { get; set; }
        public DateTime Data { get; set; }
        public ChatType Type { get; set; }
        public User User { get; set; }
    }
}
