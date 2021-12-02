using AutoMapper;
using ChatApp.BLL.DTO;
using ChatApp.BLL.DTO.UserDTO;
using ChatApp.BLL.Interfaces;
using ChatApp.DAL.Entity;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace ChatApp.Controllers
{
    [Route("api/account")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;

        public UserController(
            IMapper mapper,
            IUserService userService,
            IFileService fileService)
        {
            _mapper = mapper;
            _userService = userService;
            _fileService = fileService;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok();
        }

        [HttpGet("/get/all")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("/get/{id}")]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpPut("/edit")]
        public async Task<IActionResult> EditUserAsync(UserDetailsDTO user)
        {
            await _userService.EditUserAsync(user);
            return Ok();
        }

        [HttpGet("/getMyInfo")]
        public async Task<IActionResult> GetMyDetails()
        {
            return Ok(await _userService.GetMyInfo());
        }
    }
}

