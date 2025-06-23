using Cafe_API.Data;
using Cafe_API.Models;
using Cafe_API.Repository.IRepository;

namespace Cafe_API.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _db;
        public ProductRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
