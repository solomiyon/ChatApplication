using ChatApp.BLL.Interfaces;
using ChatApp.DAL.Entity;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly TokenManagement _tokenManagement;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _context;
        public AuthenticationService(IOptions<TokenManagement> tokenManagement, SignInManager<User> signInManager, UserManager<User> userManager, IHttpContextAccessor context)
        {
            _tokenManagement = tokenManagement.Value;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;

        }
        public async Task<string> GenerateJwtToken(User user)
        {
            var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            var claims = userPrincipal.Claims;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_tokenManagement.JwtExpireDays));

            var token = new JwtSecurityToken(
                _tokenManagement.JwtIssuer,
                _tokenManagement.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var jwttoken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwttoken;
        }

        public Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(_context.HttpContext.User);
        public async Task<IList<string>> GetCurrentUserRolesAsync(User user) => await _userManager.GetRolesAsync(user);
    }
}
