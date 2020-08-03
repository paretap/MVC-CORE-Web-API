using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WmDevWebAPI.Models
{
    public class UserVm
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
    public static class Role
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
