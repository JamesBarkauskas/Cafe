using Cafe_API.Models;
using Cafe_API.Models.Dto;
using Cafe_API.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Cafe_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        protected APIResponse _response;
        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
            this._response = new APIResponse();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var loginResponse = await _userRepo.Login(loginRequestDTO);
            // login not success..
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
            return Ok(_response);
        }
    }
}
