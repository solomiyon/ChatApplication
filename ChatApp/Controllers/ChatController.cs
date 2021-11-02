using AutoMapper;
using ChatApp.BLL.DTO;
using ChatApp.BLL.DTO.Message;
using ChatApp.BLL.Interfaces;
using ChatApp.DAL.Entity;
using ChatApp.Hubs;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        public ChatController(IChatService chatService, IMapper mapper)
        {
            _chatService = chatService;

            _mapper = mapper;
        }

        [HttpPost("/createGroup")]
        public async Task<IActionResult> CreateGroup(GroupDTO chat)
        {
            await _chatService.CreateGroup(chat);
            return Ok();
        }

        [HttpPost("/createChat")]
        public async Task<IActionResult> CreateChat(GroupDTO chat)
        {
            await _chatService.CreateChat(chat);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> EditChat(int id, Chat chat)
        {
            await _chatService.EditGroup(id, chat);
            return Ok();
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteChat(int id, Chat chat)
        {
            await _chatService.DeleteChat(id);
            return Ok();
        }

        [HttpGet("/all")]
        public async Task<IActionResult> GetChatsList()
        {
            var chats = await _chatService.GetChatsList();
            return Ok(chats);
        }

        [HttpPost("/message")]
        public async Task<IActionResult> AddMessage([FromBody] MessageViewModel message, [FromServices] IHubContext<ChatHub> chat)
        {
            if (ModelState.IsValid)
            {
                var messageDto = _mapper.Map<AddMessageDTO>(message);
                var getMessage = await _chatService.SendMessage(messageDto);
                var participants = await _chatService.GetParticipants(message.ChatId);
                var user = chat.Clients.Users(participants);
                await chat.Clients.Users(participants).SendAsync("ReceiveMessage", new
                {
                    Text = message.Text,
                    PosterFirstName = getMessage.FirstName,
                    PosterLastName = getMessage.LastName,
                    PosterLastPhoto = getMessage.ImagePath,
                    Timestamp = getMessage.Date,
                    PosterId = getMessage.CreatedById
                });
                return Ok(getMessage);
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetChat(int chatId)
        {
            if(chatId == 0)
            {
                return BadRequest();
            }
            var chat = await _chatService.GetChatAsync(chatId);
            return Ok(chat);
        }
    }
}
