using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly ApplicationDbContext _context;
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Product entity)
    {
        _context.Products.Update(entity);
    }
}
