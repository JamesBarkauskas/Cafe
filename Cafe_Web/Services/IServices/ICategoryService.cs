using Cafe_Web.Models.Dto;

namespace Cafe_Web.Services.IServices
{
    public interface ICategoryService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(CategoryCreateDTO dto);
        Task<T> UpdateAsync<T>(CategoryUpdateDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
