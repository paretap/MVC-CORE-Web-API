using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WmDevWebAPI.Models;

namespace WmDevWebAPI.Services
{
    public interface IUserService
    {
        UserVm IsValid(string username, string password);
    }
}
