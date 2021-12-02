using ChatApp.BLL.DTO;
using ChatApp.BLL.DTO.ChatDTO;
using ChatApp.BLL.DTO.Message;
using ChatApp.BLL.Mapping;
using ChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Mapping
{
    public class MappingProfile : MappingProfileBLL
    {
        public MappingProfile() : base()
        {
            CreateMap<LoginViewModel, LoginDTO>();
            CreateMap<RegisterViewModel, RegisterDTO>()
                .ForMember(u => u.ImagePath, map => map.Ignore());
            CreateMap<ChatViewModel, ChatDTO>();
            CreateMap<MessageViewModel, AddMessageDTO>();
        }
    }
}
