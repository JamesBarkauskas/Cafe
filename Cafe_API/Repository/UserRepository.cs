using AutoMapper;
using Cafe_API.Data;
using Cafe_API.Models;
using Cafe_API.Models.Dto;
using Cafe_API.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cafe_API.Repository
{
    /* Consider moving methods into a UserService folder? Repos should only
     * interact with the DB and contain
     * basic CRUD operations like GetUser, CreateUser, UpdatePass... 
     * repo should not hash passwords, validate credentials, generate jwt tokens. 
     * Repos should 'talk to db and return data'.. nothing more.. */
    public class UserRepository : Repository<AppUser>, IUserRepository
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private string secretKey;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRepository(AppDbContext db, IMapper mapper, IConfiguration config, UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager) : base(db)
        {
            _db = db;
            _mapper = mapper;
            secretKey = config.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public bool IsUnique(string username)
        {
            var user = _db.AppUsers.FirstOrDefault(u=>u.UserName== username);
            if (user == null) { return true; }
            return false;
        }
      
        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            //var user = _db.LocalUsers.FirstOrDefault(u=>u.UserName== loginRequestDTO.UserName);
            var user = _db.AppUsers.FirstOrDefault(u=>u.UserName == loginRequestDTO.UserName);
            // check password using userManager
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (user == null || isValid == false)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }


            // user exists and password is correct,
            var roles = await _userManager.GetRolesAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);   //convert key to bytes.. **use .UTF8 instead of ASCII

            // build out the token..
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                    
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // now actually create the token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // build out the responseDTO (the return type..)
            LoginResponseDTO responseDTO = new LoginResponseDTO()
            {
                User = _mapper.Map<UserDTO>(user),
                Token = tokenHandler.WriteToken(token),
                Role = roles.FirstOrDefault()
            };
            return responseDTO;

        }

        public async Task<UserDTO> Register(RegistrationRequestDTO registerRequestDTO)
        {
            AppUser user = new()
            {
                UserName = registerRequestDTO.UserName,
                Name = registerRequestDTO.Name,
                NormalizedEmail = registerRequestDTO.UserName.ToUpper() + "@gmail.com",
                Email = registerRequestDTO.UserName + "@gmail.com"
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerRequestDTO.Password);
                if (result.Succeeded)
                {
                    // check if roles exist.. wont exist first time so will only execute the first time..
                    // 'hacky' way of adding roles, rather do it by seeding the db..
                    //if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    //{
                    //    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    //    await _roleManager.CreateAsync(new IdentityRole("customer"));
                    //}

                    await _userManager.AddToRoleAsync(user, "Customer");
                    var userToReturn = _db.AppUsers.FirstOrDefault(u=>u.UserName== registerRequestDTO.UserName);
                    return _mapper.Map<UserDTO>(userToReturn);
                }

                // todo: else - return error on register page..
            }
            catch (Exception ex)
            {

            }
            return new UserDTO();
        }
    }
}
