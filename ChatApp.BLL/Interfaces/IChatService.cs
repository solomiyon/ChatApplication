using ChatApp.BLL.DTO;
using ChatApp.BLL.DTO.Message;
using ChatApp.DAL.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.BLL.Interfaces
{
    public interface IChatService
    {
        Task CreateGroup(GroupDTO groupDTO);
        Task CreateChat(GroupDTO chatDTO);
        Task EditGroup(int id, Chat chat);
        Task DeleteChat(int id);
        Task<List<GetChatsDTO>> GetChatsList();
        Task<GetMessageDTO> SendMessage(AddMessageDTO messageDTO);
        Task<List<string>> GetParticipants(int chatId);
        Task<Chat> GetChatAsync(int chatId);
    }
}
