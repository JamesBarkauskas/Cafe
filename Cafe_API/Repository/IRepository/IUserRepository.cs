using Cafe_API.Models;
using Cafe_API.Models.Dto;

namespace Cafe_API.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUnique(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<LocalUser> Register(RegistrationRequestDTO registerRequestDTO);
    }
}
