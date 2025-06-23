using Cafe_Utility;
using Cafe_Web.Models;
using Cafe_Web.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Cafe_Web.Services
{
    public class BaseService : IBaseService
    {
        //public APIResponse responseModel { get; set; }
        private readonly IHttpClientFactory _httpClient;
        public BaseService(IHttpClientFactory httpClient)
        {
            //responseModel = new APIResponse();
            _httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(APIRequest request)
        {
            try
            {
                var client = _httpClient.CreateClient("CafeAPI");
                // build out request
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(request.Url);
                if (request.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data),
                        Encoding.UTF8, "application/json");
                }

                // assign http verb..
                switch (request.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                // request is configured, now to send it
                //HttpResponseMessage response = null;
                //response = await client.SendAsync(message);

                // implement Token

                HttpResponseMessage responseMessage = await client.SendAsync(message);
                var apiContent = await responseMessage.Content.ReadAsStringAsync();
                try
                {
                    APIResponse apiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if (apiResponse!=null && (responseMessage.StatusCode == HttpStatusCode.BadGateway 
                        || apiResponse.StatusCode == HttpStatusCode.NotFound))
                    {
                        apiResponse.StatusCode = HttpStatusCode.BadRequest;
                        apiResponse.IsSuccess = false;
                        var res = JsonConvert.SerializeObject(apiResponse);
                        var returnObj = JsonConvert.DeserializeObject<T>(res);
                        return returnObj;
                    }
                }
                catch (Exception ex)
                {
                    //var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    //return exceptionResponse;
                }
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;


            } catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    Errors = new List<string>() { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var apiResponseObj = JsonConvert.DeserializeObject<T>(res);
                return apiResponseObj;
            }
        }
        
    }
}
