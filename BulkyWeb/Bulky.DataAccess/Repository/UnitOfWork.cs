using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
namespace Bulky.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public ICategoryRepository Category { get; private set; }
    public IProductRepository Product { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Category = new CategoryRepository(_context);
        Product = new ProductRepository(_context);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
