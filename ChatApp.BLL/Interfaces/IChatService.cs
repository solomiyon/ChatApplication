using ChatApp.BLL.DTO;
using ChatApp.BLL.DTO.ChatDTO;
using ChatApp.BLL.DTO.Message;
using ChatApp.DAL.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.BLL.Interfaces
{
    public interface IChatService
    {
        Task CreateGroupAsync(GroupDTO groupDTO);
        Task CreateChatAsync(string userId);
        Task EditGroupAsync(int id, Chat chat);
        Task DeleteChatAsync(int id);
        Task<List<GetChatsDTO>> GetChatsListAsync();
        Task<GetMessageDTO> SendMessageAsync(AddMessageDTO messageDTO);
        Task<List<string>> GetParticipantsAsync(int chatId);
        Task<GetChatDTO> GetChatAsync(int chatId);
        Task DeleteMessageAsync(int id);
        Task AddChanelUsers(AddChanelUsersDTO chanel);
    }
}
