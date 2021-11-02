using ChatApp.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.BLL.DTO
{
    public class GroupDTO
    {
        public string Name { get; set; }
        public DateTime Data { get; set; }
        public ChatType Type { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
