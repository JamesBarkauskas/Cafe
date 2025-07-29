using Cafe_API.Models;
using Cafe_API.Models.Dto;

namespace Cafe_API.Repository.IRepository
{
    public interface IUserRepository : IRepository<AppUser>
    {
        bool IsUnique(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegistrationRequestDTO registerRequestDTO);
    }
}
