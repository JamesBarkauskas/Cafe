using Cafe_Utility;
using Cafe_Web.Models;
using Cafe_Web.Models.Dto;
using Cafe_Web.Services.IServices;

namespace Cafe_Web.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _httpClient;
        private string cafeUrl;
        public AuthService(IHttpClientFactory httpClient, IConfiguration config) : base(httpClient)
        {
            _httpClient = httpClient;
            cafeUrl = config.GetValue<string>("ServiceUrls:CafeAPI");
        }

        public Task<T> LoginAsync<T>(LoginRequestDTO obj)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = cafeUrl + "/api/user/login",
                Data = obj
            });
        }

        public Task<T> RegisterAsync<T>(RegistrationRequestDTO obj)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType= SD.ApiType.POST,   
                Url = cafeUrl + "/api/user/register",
                Data = obj
            });
        }

        public Task<T> GetAllUsers<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = cafeUrl +"/api/user/GetUsers",
                Token = token
            });
        }

        public Task<T> GetUser<T>(string id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = cafeUrl + "/api/user?id=" + id,
                Token = token
            });
        }

        public Task<T> DeleteUser<T>(string id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = cafeUrl + "/api/user?id=" + id,
                Token = token
            });
        }
    }
}
