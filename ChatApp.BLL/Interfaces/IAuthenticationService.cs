using ChatApp.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BLL.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> GenerateJwtToken(User user);
        Task<User> GetCurrentUserAsync();
        Task<IList<string>> GetCurrentUserRolesAsync(User user);
    }
}
