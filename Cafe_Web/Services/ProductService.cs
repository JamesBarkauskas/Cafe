using Cafe_Utility;
using Cafe_Web.Models;
using Cafe_Web.Models.Dto;
using Cafe_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Cafe_Web.Services
{
    public class ProductService : BaseService, IProductService
    {
        //private readonly IHttpClientFactory _httpClient;
        private string _cafeUrl;
        public ProductService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            //_httpClient = httpClientFactory;
            _cafeUrl = configuration.GetValue<string>("ServiceUrls:CafeAPI");
        }

        public Task<T> CreateAsync<T>(ProductCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = _cafeUrl + "/api/ProductAPI",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = _cafeUrl + "/api/ProductAPI/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = _cafeUrl + "/api/ProductAPI",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = _cafeUrl + "/api/ProductAPI/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(ProductUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = _cafeUrl + "/api/ProductAPI/" + dto.Id,
                Token = token
            });
        }
    }
}
