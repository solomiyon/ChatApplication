using AutoMapper;
using ChatApp.BLL.DTO;
using ChatApp.BLL.Interfaces;
using ChatApp.DAL.Entity;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace ChatApp.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        //private readonly IAuthenticationService _authentication;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;

        public AccountController(
            IMapper mapper,
            IUserService userService, 
            IFileService fileService)
        {
            _mapper = mapper;
            _userService = userService;
            _fileService = fileService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var loginDto = model;
                var token = await _userService.Login(loginDto);
                if (!string.IsNullOrEmpty(token)) return Ok(new { Token = token });
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<RegisterDTO>(model);

                if (model.Image != null)
                {
                    var imagePath = await _fileService.SaveFile(model.Image.OpenReadStream(),
                        Path.GetExtension(model.Image.FileName));
                    user.ImagePath = imagePath.ToString();
                }
                await _userService.Register(user);
                return Ok();
            }

            return BadRequest();
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

        [HttpGet("/get")]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpPut("/edit")]
        public async Task<IActionResult> EditUser(string id, User user)
        {
            await _userService.EditUserAsync(id, user);
            return Ok();
        }
    }
}

