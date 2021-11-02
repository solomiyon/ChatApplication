using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.BLL.DTO
{
    public class RegisterDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public string ImagePath { get; set; }
    }
}
