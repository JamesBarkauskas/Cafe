using Cafe_Utility;
using Cafe_Web.Models;
using Cafe_Web.Models.Dto;
using Cafe_Web.Services;
using Cafe_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Cafe_Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // ** is it neccessary to return an obj to the view..?
            LoginRequestDTO obj = new();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO obj)
        {
            var response = await _authService.LoginAsync<APIResponse>(obj);
            if (response != null && response.IsSuccess)
            {
                LoginResponseDTO model = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(
                    response.Result));

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(model.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);   // same as doing 'new ClaimsIdentity("Cookies");
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u=>u.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u=>u.Type=="role")?.Value));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // once signed in, retrieve token and store in user session
                HttpContext.Session.SetString(SD.SessionToken, model.Token);

                return RedirectToAction("Index", "Home");

            }
            else
            {
                ModelState.AddModelError("CustomError", response.Errors.FirstOrDefault());
                return View(obj);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationRequestDTO model)
        {
            APIResponse response = await _authService.RegisterAsync<APIResponse>(model);
            if (response != null)
            {
                // ** TODO: have it sign user in and send them home page rather than making them sign in..
                var loginModel = new LoginRequestDTO
                {
                    UserName = model.UserName,
                    Password = model.Password
                };
                APIResponse loginResponse = await _authService.LoginAsync<APIResponse>(loginModel);
                if (loginResponse != null && loginResponse.IsSuccess)
                {
                    var loginResult = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(
                        loginResponse.Result));

                    // get token from loginResult
                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(loginResult.Token);

                    // create identity and add claims and principal
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u=>u.Type=="unique_name").Value));
                    identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role")?.Value));

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // once signed in, retrieve token and store in user session
                    // want to store the jwt token into user's session.. allows user to make auth requests..(if admin)
                    HttpContext.Session.SetString(SD.SessionToken, loginResult.Token);

                    return RedirectToAction("Index", "Home");
                }

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, "");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<UserDTO> users = new();
            var response = await _authService.GetAllUsers<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                users = JsonConvert.DeserializeObject<List<UserDTO>>(Convert.ToString(response.Result));
            }
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            UserDTO user = new();
            var response = await _authService.GetUser<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                user = JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(response.Result));
            }
            return View(user);

        }
    }
}
