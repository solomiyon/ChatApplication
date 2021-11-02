using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.BLL.DTO
{
    public class GetChatsDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImagePath { get; set; }
        public string UserId { get; set; }
        public string LastMessage { get; set; }
        public int ChatId { get; set; }
        public DateTime Date { get; set; }
    }
}
