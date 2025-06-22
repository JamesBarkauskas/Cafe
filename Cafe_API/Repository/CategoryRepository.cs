using Cafe_API.Data;
using Cafe_API.Models;
using Cafe_API.Repository.IRepository;

namespace Cafe_API.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _db;
        public CategoryRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
