using AutoMapper;
using ChatApp.BLL.DTO;
using ChatApp.BLL.DTO.ChatDTO;
using ChatApp.BLL.DTO.Message;
using ChatApp.BLL.Interfaces;
using ChatApp.DAL.Entity;
using ChatApp.Hubs;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.Controllers
{
    [Route("api/chat")]
    [Authorize]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;
        IHubContext<ChatHub> _chatHub;

        public ChatController(IChatService chatService, IMapper mapper, IHubContext<ChatHub> chatHub)
        {
            _chatService = chatService;
            _mapper = mapper;
            _chatHub = chatHub;
        }

        [HttpPost("/createGroup")]
        public async Task<IActionResult> CreateGroupAsync(GroupDTO chat)
        {
            if (ModelState.IsValid)
            {
                await _chatService.CreateGroupAsync(chat);
                return Ok();
            }
            return BadRequest();
            
        }

        [HttpPost("/createChat/{userId}")]
        public async Task<IActionResult> CreateChatAsync(string userId)
        {
            if (ModelState.IsValid)
            {
                await _chatService.CreateChatAsync(userId);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("/addUsersForChat")]
        public async Task<IActionResult> AddUsersForChanel([FromBody] AddChanelUsersDTO chanel)
        {
            if (ModelState.IsValid)
            {
                await _chatService.AddChanelUsers(chanel);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> EditChatAsync(int id, Chat chat)
        {
            await _chatService.EditGroupAsync(id, chat);
            return Ok();
        }

        [HttpDelete("/deleteChat/{id}")]
        public async Task<IActionResult> DeleteChatAsync(int id)
        {
            await _chatService.DeleteChatAsync(id);
            return Ok();
        }

        [HttpGet("/all")]
        public async Task<IActionResult> GetChatsListAsync()
        {
            var chats = await _chatService.GetChatsListAsync();
            return Ok(chats);
        }

        [HttpPost("/message")]
        public async Task<IActionResult> AddMessageAsync([FromBody] MessageViewModel message)
        {
            if (ModelState.IsValid)
            {
                var messageDto = _mapper.Map<AddMessageDTO>(message);
                var getMessage = await _chatService.SendMessageAsync(messageDto);
                var participants = await _chatService.GetParticipantsAsync(message.ChatId);
                var user = _chatHub.Clients.Users(participants);
                await _chatHub.Clients.Users(participants).SendAsync("ReceiveMessage", getMessage);
                return Ok(getMessage);
            }
            return BadRequest();
        }

        [HttpGet("/getChat/{chatId}")]
        public async Task<IActionResult> GetChatAsync(int chatId)
        {
            if(chatId == 0)
            {
                return BadRequest();
            }
            var chat = await _chatService.GetChatAsync(chatId);
            return Ok(chat);
        }

        [HttpDelete("/deleteMessage/{id}")]
        public async Task<IActionResult> RemoveMessageAsync(int id)
        {
            await _chatService.DeleteMessageAsync(id);
            return Ok();
        }
    }
}
