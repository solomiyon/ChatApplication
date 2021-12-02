using AutoMapper;
using ChatApp.BLL.DTO;
using ChatApp.BLL.DTO.UserDTO;
using ChatApp.BLL.Interfaces;
using ChatApp.DAL.Entity;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace ChatApp.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
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
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var loginDto = model;
                var token = await _userService.LoginAsync(loginDto);
                if (!string.IsNullOrEmpty(token)) return Ok(new { Token = token });
            }
            return BadRequest();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
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
                await _userService.RegisterAsync(user);
                return Ok();
            }

            return BadRequest();
        }
    }
}
