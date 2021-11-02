using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.BLL.DTO
{
    public class EditUserDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ImagePath { get; set; }
    }
}
