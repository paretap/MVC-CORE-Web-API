using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WmDevWebAPI.Helpers;
using WmDevWebAPI.Models;

namespace WmDevWebAPI.Services
{
    public class UserService : IUserService
    {
        readonly List<UserVm> _users = new List<UserVm>();

        public UserService()
        {
            
        }

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _users.Add(new UserVm { Id = 1, FirstName = "Jim", LastName = "Kery", Password = "pass", Role = "Admin", Username = "Admin" });
            _users.Add(new UserVm { Id = 1, FirstName = "Kim", LastName = "Jhon", Password = "pass", Role = "User", Username = "User" });
            _appSettings = appSettings.Value;
        }


        public UserVm IsValid(string username, string password)
        {
            UserVm user = null;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                // In Case of Form 
                user = DoCheckInDB(username, password);

                // In Case of Windows AD
                var adInfo  = ADHelper.DoAuthentication(username, password);

            }

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("UserName", user.Username),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        private UserVm DoCheckInDB(string username, string password)
        {
           return _users.SingleOrDefault(x => x.Username.Equals(username) && x.Password.Equals(password));
        }
    }
}
