using AutoMapper;
using ChatApp.BLL.DTO;
using ChatApp.BLL.DTO.ChatDTO;
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
        public async Task CreateGroupAsync(GroupDTO groupDTO)
        {
            var chat = _mapper.Map<Chat>(groupDTO);
            var admin = await _authentication.GetCurrentUserAsync();
            chat.AdminId = admin.Id;
            chat.Data = DateTime.Now;
            await _unitOfWork.Repository<Chat>().AddAsync(chat);
            await _unitOfWork.SaveChangesAsync();

            ChatUser adminUser = new ChatUser
            {
                UserId = admin.Id,
                ChatId = chat.Id
            };
            await _unitOfWork.Repository<ChatUser>().AddAsync(adminUser);
            await _unitOfWork.SaveChangesAsync();

            if (groupDTO.Users != null)
            {
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
        }

        public async Task CreateChatAsync(string userId)
        {
            Chat chat = new Chat();
            chat.Data = DateTime.Now;
            chat.Type = 0;
            await _unitOfWork.Repository<Chat>().AddAsync(chat);
            await _unitOfWork.SaveChangesAsync();

            var creator = await _authentication.GetCurrentUserAsync();

            ChatUser creatorUser = new ChatUser
            {
                UserId = creator.Id,
                ChatId = chat.Id
            };
            ChatUser user = new ChatUser
            {
                UserId = userId,
                ChatId = chat.Id
            };
            await _unitOfWork.Repository<ChatUser>().AddAsync(creatorUser);
            await _unitOfWork.Repository<ChatUser>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EditGroupAsync(int id, Chat chat)
        {
            await _unitOfWork.Repository<Chat>().AddAsync(chat);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteChatAsync(int id)
        {
            var chat = await _unitOfWork.Repository<Chat>().GetAsync(c => c.Id == id);
            _unitOfWork.Repository<Chat>().Remove(chat);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<GetChatsDTO>> GetChatsListAsync()
        {
            var user = await _authentication.GetCurrentUserAsync();
            var chatsList = await _unitOfWork.Repository<ChatUser>().GetAllAsync(cu => cu.UserId == user.Id);
            var chatsId = chatsList.Select(x => x.ChatId).ToList();

            var chatsDto = new List<GetChatsDTO>();

            foreach (var chatId in chatsId)
            {
                var chat = await _unitOfWork.Repository<Chat>().GetIncludingAll(c => c.Id == chatId);

                var chatDTO = new GetChatsDTO();
                chatDTO = _mapper.Map<GetChatsDTO>(chat);

                if (chat.Type == 0)
                {
                    var participantId = chat.Participants.Where(u => u.UserId != user.Id).FirstOrDefault().UserId;
                    var participantInfo = await _unitOfWork.Repository<User>().GetAsync(u => u.Id == participantId);
                    chatDTO = _mapper.Map<GetChatsDTO>(participantInfo);
                    chatDTO.UserId = participantInfo.Id;
                }
                
                var message = chat.Messages.LastOrDefault();
                if(message != null)
                {
                    chatDTO.LastMessage = message.Text;
                    chatDTO.Date = message.Date;
                }
                else
                {
                    chatDTO.LastMessage = " ";
                }
                chatDTO.ChatId = chatId;
                chatsDto.Add(chatDTO);
            }
            return chatsDto;
        }

        public async Task<GetChatDTO> GetChatAsync(int chatId)
        {
            var chat = await _unitOfWork.Repository<Chat>().GetAsync(c => c.Id == chatId);
            var messages = await _unitOfWork.Repository<Message>().GetAllAsync(m => m.ChatId == chatId);

            var chatDTO = _mapper.Map<GetChatDTO>(chat);

            var participants = await _unitOfWork.Repository<ChatUser>().GetAllAsync(c => c.ChatId == chatId);

            var currentUser = await _authentication.GetCurrentUserAsync();
            chatDTO.CurrentUserId = currentUser.Id;

            foreach(var partisipant in participants)
            {
                if(currentUser.Id != partisipant.UserId)
                {
                    var user = await _unitOfWork.Repository<User>().GetAsync(u => u.Id == partisipant.UserId);
                    chatDTO.UserId = user.Id;
                    chatDTO.FirstName = user.FirstName;
                    chatDTO.LastName = user.LastName;
                    chatDTO.ImagePath = user.ImagePath;
                }
            }
           
            var messagesDTO = new List<GetMessageDTO>();
            foreach (var message in messages)
            {
                var messageDTO = _mapper.Map<GetMessageDTO>(message);
                var user = await _unitOfWork.Repository<User>().GetAsync(u => u.Id == messageDTO.UserId);
                messageDTO.FirstName = user.FirstName;
                messageDTO.LastName = user.LastName;
                messageDTO.ImagePath = user.ImagePath;
                messagesDTO.Add(messageDTO);
            }
            chatDTO.Messages = messagesDTO
                .OrderBy(m => m.Date)
                .ToList();

            return chatDTO;
        }

        public async Task<GetMessageDTO> SendMessageAsync(AddMessageDTO messageDTO)
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

        public async Task<List<string>> GetParticipantsAsync(int chatId)
        {
            var chat = await _unitOfWork.Repository<Chat>().GetIncludingAll(c => c.Id == chatId);
            if (chat == null) return null;

            var receiversId = chat.Participants.Select(u => u.UserId).ToList();
            return receiversId;
        }

        public async Task DeleteMessageAsync(int id)
        {
            var message = await _unitOfWork.Repository<Message>().GetAsync(m => m.Id == id);
            _unitOfWork.Repository<Message>().Remove(message);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddChanelUsers(AddChanelUsersDTO chanel)
        {
            foreach (User user in chanel.Users)
            {
                ChatUser chatUser = new ChatUser
                {
                    UserId = user.Id,
                    ChatId = chanel.Id
                };
                await _unitOfWork.Repository<ChatUser>().AddAsync(chatUser);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
