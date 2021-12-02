using ChatApp.BLL.DTO.Message;
using ChatApp.DAL.Entity;
using System;
using System.Collections.Generic;

namespace ChatApp.BLL.DTO.ChatDTO
{
    public class GetChatDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public ChatType Type { get; set; }
        public string ImagePath { get; set; }
        public string CurrentUserId { get; set; }
        public string UserId { get; set; }
        public ICollection<GetMessageDTO> Messages { get; set; }
    }
}
