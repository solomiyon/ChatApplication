using AutoMapper;
using ChatApp.BLL.DTO;
using ChatApp.BLL.DTO.Message;
using ChatApp.DAL.Entity;

namespace ChatApp.BLL.Mapping
{
    public class MappingProfileBLL : Profile
    {
        public MappingProfileBLL()
        {
            CreateMap<RegisterDTO, User>()
                .ForMember(u => u.ImagePath, map => map.Ignore())
                .ForMember(u => u.UserName, mqp => mqp.MapFrom(vm => vm.Email)).ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, LoginDTO>().ReverseMap();
            CreateMap<Chat, ChatDTO>().ReverseMap();
            CreateMap<Chat, GroupDTO>().ReverseMap();
            CreateMap<User, GetChatsDTO>().ReverseMap();
            CreateMap<Message, AddMessageDTO>().ReverseMap();
            CreateMap<Message, GetMessageDTO>().ReverseMap();
        }      
    }
}
