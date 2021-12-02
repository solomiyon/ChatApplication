using ChatApp.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.BLL.DTO.ChatDTO
{
    public class AddChanelUsersDTO
    {
        public int Id { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
