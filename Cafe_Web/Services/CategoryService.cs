using Cafe_Utility;
using Cafe_Web.Models;
using Cafe_Web.Models.Dto;
using Cafe_Web.Services.IServices;

namespace Cafe_Web.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly IHttpClientFactory _httpClient;
        private string cafeUrl;
        public CategoryService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClient = httpClientFactory;
            cafeUrl = configuration.GetValue<string>("ServiceUrls:CafeAPI");
        }
        public Task<T> CreateAsync<T>(CategoryCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = cafeUrl + "/api/categoryAPI"
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = cafeUrl + "/api/categoryAPI/" + id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = cafeUrl + "/api/categoryAPI"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = cafeUrl + "/api/categoryAPI/" + id
            });
        }

        public Task<T> UpdateAsync<T>(CategoryUpdateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = cafeUrl + "/api/categoryAPI/" + dto.Id
            });
        }
    }
}
