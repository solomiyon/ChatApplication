using AutoMapper;
using ChatApp.BLL.DTO;
using ChatApp.BLL.DTO.Message;
using ChatApp.BLL.Interfaces;
using ChatApp.DAL.Entity;
using ChatApp.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.BLL.Service
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authentication;
        public ChatService(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticationService authentication)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authentication = authentication;
        }
        public async Task CreateGroup(GroupDTO groupDTO)
        {
            var chat = _mapper.Map<Chat>(groupDTO);
            await _unitOfWork.Repository<Chat>().AddAsync(chat);
            await _unitOfWork.SaveChangesAsync();

            foreach (User user in groupDTO.Users)
            {
                ChatUser chatUser = new ChatUser
                {
                    UserId = user.Id,
                    ChatId = chat.Id
                };
                await _unitOfWork.Repository<ChatUser>().AddAsync(chatUser);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task CreateChat(GroupDTO chatDTO)
        {
            var chat = _mapper.Map<Chat>(chatDTO);
            chat.Type = 0;
            await _unitOfWork.Repository<Chat>().AddAsync(chat);
            await _unitOfWork.SaveChangesAsync();

            var creator = _authentication.GetCurrentUserAsync();

            ChatUser creatorUser = new ChatUser
            {
                UserId = creator.Id.ToString(),
                ChatId = chat.Id
            };
            ChatUser user = new ChatUser
            {
                UserId = chatDTO.Users.FirstOrDefault().Id,
                ChatId = chat.Id
            };
            await _unitOfWork.Repository<ChatUser>().AddAsync(creatorUser);
            await _unitOfWork.Repository<ChatUser>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EditGroup(int id, Chat chat)
        {
            await _unitOfWork.Repository<Chat>().AddAsync(chat);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteChat(int id)
        {
            var chat = await _unitOfWork.Repository<Chat>().GetAsync(c => c.Id == id);
            _unitOfWork.Repository<Chat>().Remove(chat);
        }

        public async Task<List<GetChatsDTO>> GetChatsList()
        {
            var user = await _authentication.GetCurrentUserAsync();
            var chatsList = await _unitOfWork.Repository<ChatUser>().GetAllAsync(cu => cu.UserId == user.Id);
            var chatsId = chatsList.Select(x => x.ChatId).ToList();

            var chatsDto = new List<GetChatsDTO>();

            foreach (var chatId in chatsId)
            {
                var chat = await _unitOfWork.Repository<Chat>().GetIncludingAll(c => c.Id == chatId);
                var participantId = chat.Participants.Where(u => u.UserId != user.Id).FirstOrDefault().UserId;
                var participantInfo = await _unitOfWork.Repository<User>().GetAsync(u => u.Id == participantId);

                var GetChatDto = new GetChatsDTO();
                GetChatDto = _mapper.Map<GetChatsDTO>(participantInfo);
                GetChatDto.UserId = participantInfo.Id;
                var message = chat.Messages.LastOrDefault();
                GetChatDto.LastMessage = message.Text;
                GetChatDto.ChatId = chatId;
                GetChatDto.Date = message.Date;
                chatsDto.Add(GetChatDto);
            }
            return chatsDto;
        }

        public async Task<Chat> GetChatAsync(int chatId)
        {
            var chat = await _unitOfWork.Repository<Chat>().GetAsync(c => c.Id == chatId);
            var messages = await _unitOfWork.Repository<Message>().GetAllAsync(m => m.ChatId == chatId);
            chat.Messages = messages.ToList();
            return chat;
        }

        public async Task<GetMessageDTO> SendMessage(AddMessageDTO messageDTO)
        {
            var chat = await _unitOfWork.Repository<Chat>().GetAsync(c => c.Id == messageDTO.ChatId);
            if (chat == null) return null;

            var creator = await _authentication.GetCurrentUserAsync();
            messageDTO.UserId = creator.Id;
            messageDTO.Date = DateTime.Now;
            var message = _mapper.Map<Message>(messageDTO);
            await _unitOfWork.Repository<Message>().AddAsync(message);
            await _unitOfWork.SaveChangesAsync();

            var getMessage = _mapper.Map<GetMessageDTO>(message);
            getMessage.FirstName = creator.FirstName;
            getMessage.LastName = creator.LastName;
            getMessage.ImagePath = creator.ImagePath;
            return getMessage;
        }

        public async Task<List<string>> GetParticipants(int chatId)
        {
            var chat = await _unitOfWork.Repository<Chat>().GetIncludingAll(c => c.Id == chatId);
            if (chat == null) return null;

            var receiversId = chat.Participants.Select(u => u.UserId).ToList();
            return receiversId;
        }
    }
}
