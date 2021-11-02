using AutoMapper;
using ChatApp.BLL.DTO;
using ChatApp.BLL.Interfaces;
using ChatApp.DAL.Entity;
using ChatApp.DAL.UnitOfWork;
using Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.BLL.Service
{
    public class UserService : IUserService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationService _authentication;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _env;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IAuthenticationService authentication, IMapper mapper, 
            IFileService fileService, IUnitOfWork unitOfWork, IHostingEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authentication = authentication;
            _mapper = mapper;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _env = env;
        }

        public async Task<string> Login(LoginDTO model)
        {
            var user = await _unitOfWork.Repository<User>().GetAsync(u => u.Email == model.Email);
            //  if (!user.EmailConfirmed) return null;
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                var token = await _authentication.GenerateJwtToken(appUser);
                return token;
            }
            return null;
        }

        public async Task Register(RegisterDTO model)
        {
            var user = _mapper.Map<User>(model);
            user.ImagePath = model.ImagePath;
            var result = await _userManager.CreateAsync(user, model.Password);
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _unitOfWork.Repository<User>().GetAsync(t => t.Id == id);
            _unitOfWork.Repository<User>().Remove(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            if (users != null)
            {
                return users.ToList();
            }
            return null;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _unitOfWork.Repository<User>().GetAsync(t => t.Id == id);
        }

        public async Task EditUserAsync(string id, User user)
        {
            var updatedUser = await _unitOfWork.Repository<User>().GetAsync(u => u.Id == id);
            updatedUser.FirstName = user.FirstName;
            updatedUser.LastName = user.LastName;
            updatedUser.PhoneNumber = user.PhoneNumber;
            updatedUser.UserName = user.UserName;
            updatedUser.UserRole = user.UserRole;
            updatedUser.Email = user.Email;
            updatedUser.ImagePath = user.ImagePath;
            await _unitOfWork.Repository<User>().UpdateAsync(updatedUser);
            await _unitOfWork.SaveChangesAsync();
            return;
        }
    }
}
