using Leo.Model;
using Leo.Model.Domains;

namespace Leo.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthModel> Register(RegisterModel model);
        Task<AuthModel> Login(LoginModel model);
        Task<bool> RemoveUser(string id);
    }
}
