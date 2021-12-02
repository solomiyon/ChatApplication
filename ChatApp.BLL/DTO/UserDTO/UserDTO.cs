using ChatApp.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.BLL.DTO.UserDTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public UserRole UserRole { get; set; }
        public byte[] Avatar { get; set; }
    }
}
