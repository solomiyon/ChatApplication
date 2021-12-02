using ChatApp.BLL.DTO;
using ChatApp.BLL.DTO.UserDTO;
using ChatApp.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BLL.Interfaces
{
    public interface IUserService
    {
        Task DeleteUserAsync(string id);
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task EditUserAsync(UserDetailsDTO user);
        Task<string> LoginAsync(LoginDTO model);
        Task RegisterAsync(RegisterDTO model);
        Task<UserDetailsDTO> GetMyInfo();
    }
}
