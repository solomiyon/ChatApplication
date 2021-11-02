using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.BLL.DTO.Message
{
    public class AddMessageDTO
    {
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public int ChatId { get; set; }
    }
}
