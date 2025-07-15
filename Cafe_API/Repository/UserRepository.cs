using AutoMapper;
using Cafe_API.Data;
using Cafe_API.Models;
using Cafe_API.Models.Dto;
using Cafe_API.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cafe_API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private string secretKey;
        public UserRepository(AppDbContext db, IMapper mapper, IConfiguration config) 
        {
            _db = db;
            _mapper = mapper;
            secretKey = config.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUnique(string username)
        {
            var name = _db.LocalUsers.FirstOrDefault(u=>u.UserName== username);
            if (name == null) { return true; }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.LocalUsers.FirstOrDefault(u=>u.UserName== loginRequestDTO.UserName);
            if (user.Password != loginRequestDTO.Password)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }
            // user exists,
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);   //convert key to bytes

            // build out the token..
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // now actually create the token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // build out the responseDTO (the return type..)
            LoginResponseDTO responseDTO = new LoginResponseDTO()
            {
                User = user,
                Token = tokenHandler.WriteToken(token)
            };
            return responseDTO;

        }

        public async Task<LocalUser> Register(RegistrationRequestDTO registerRequestDTO)
        {
            LocalUser user = new();
            if (IsUnique(registerRequestDTO.UserName))
            {
                user.UserName = registerRequestDTO.UserName;
                user.Password = registerRequestDTO.Password;
                user.Name = registerRequestDTO.Name;
                user.Role = registerRequestDTO.Role;

                _db.LocalUsers.Add(user);
                await _db.SaveChangesAsync();
            }
            return user;
        }
    }
}
