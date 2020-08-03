using WmDevWebAPI.Models;

namespace WmDevWebAPI.Services
{
    public interface IUserService
    {
        UserVm IsValid(string username, string password);
    }
}
