using Core.Entities;
using Data.Context;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Concrete;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    private readonly AppDbContext _context;
    protected readonly DbSet<Product> _dbTable;
    public ProductRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _dbTable = context.Set<Product>();
    }
    public override List<Product> GetAll()
    {
        return _dbTable.Include(p => p.Seller).ToList();
    }
}
