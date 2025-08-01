using AutoMapper;
using Cafe_API.Data;
using Cafe_API.Models;
using Cafe_API.Models.Dto;
using Cafe_API.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Cafe_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IUserRepository _userRepo;
        protected APIResponse _response;    // consider making _response method-level, not class-lvel...
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepo, IMapper mapper, AppDbContext db)
        {
            _db = db;
            _userRepo = userRepo;
            _mapper = mapper;
            this._response = new APIResponse();
        }

        // Retrieve all users for user management..
        // wil want to create UserDTO to expose only neccesary data like name..
        // Todo: dont return list of AppUsers(may return extra data..), instead map to a UserDTO...
        // so update api to explicityl map to userDTO obj..
        [HttpGet("GetUsers")]
        public async Task<ActionResult<APIResponse>> GetUsers()
        {
            var roles = _db.Roles.ToList(); // gives us roleId, roleName
            var users = await _userRepo.GetAllAsync();

            // will prob need to fix how i get the roles..(shouldnt have 2 nested for loops..)
            foreach (var userRole in _db.UserRoles)
            {
                foreach (var user in users)
                {
                    if (userRole.UserId == user.Id)
                    {

                        user.Role = userRole.RoleId;
                    }
                }
            }

            foreach(var user in users)
            {
                foreach(var role in roles)
                {
                    if (role.Id == user.Role)
                    {
                        user.Role = role.Name;
                    }
                }
            }

            //List<UserDTO> userDtos = _mapper.Map<UserDTO>(users);
            _response.Result = users;
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetUser(string id)
        {
            var user = await _userRepo.GetAsync(u=>u.Id == id);
            if (user != null)
            {
                _response.Result = user;
                _response.StatusCode=System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            return BadRequest();
        }

        // ** add status codes to both methods **
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var loginResponse = await _userRepo.Login(loginRequestDTO);
            // login not success..
            // ** do i check if the response is null or resposne.user??? **
            // ** set breakpoint and test... **
            if (loginResponse == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("Invalid credentials.");
                return BadRequest(_response);
            }

            // login successful
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registerDTO)
        {
            // make sure username is unique
            bool isUnique = _userRepo.IsUnique(registerDTO.UserName);
            if (!isUnique)
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("Username not unique");
                return BadRequest(_response);
            }
            var user = await _userRepo.Register(registerDTO);
            if (user == null)
            {
                _response.StatusCode=System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("Error while registering");
                return BadRequest(_response);
            }

            _response.StatusCode= System.Net.HttpStatusCode.OK;
            _response.Result = user;
            return Ok(_response);
        }
    }
}
