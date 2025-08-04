using Cafe_Web.Models.Dto;

namespace Cafe_Web.Services.IServices
{
    public interface IAuthService : IBaseService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO obj);
        Task<T> RegisterAsync<T>(RegistrationRequestDTO obj);
        Task<T> GetAllUsers<T>(string token);
        Task<T> GetUser<T>(string id, string token);
        Task<T> DeleteUser<T>(string id, string token);
    }
}
