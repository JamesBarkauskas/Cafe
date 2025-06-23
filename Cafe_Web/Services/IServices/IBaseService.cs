using Cafe_Web.Models;

namespace Cafe_Web.Services.IServices
{
    public interface IBaseService
    {
        //APIResponse responseModel { get; set; }
        Task<T> SendAsync<T>(APIRequest request);
    }
}
