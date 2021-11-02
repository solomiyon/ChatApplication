using ChatApp.BLL.DTO;
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
        Task EditUserAsync(string id, User user);

        Task<string> Login(LoginDTO model);
        Task Register(RegisterDTO model);
    }
}
