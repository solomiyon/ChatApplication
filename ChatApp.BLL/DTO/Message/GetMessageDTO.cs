using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.BLL.DTO.Message
{
    public class GetMessageDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImagePath { get; set; }
        public string Text { get; set; }
        public int ChatId { get; set; }
    }
}
