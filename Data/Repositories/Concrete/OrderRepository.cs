using Core.Entities;
using Data.Context;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Concrete;
public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    private readonly AppDbContext _context;
    protected readonly DbSet<Order> _dbTable;
    public OrderRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _dbTable = context.Set<Order>();
    }
    public override List<Order> GetAll()
    {
        return _dbTable.Include(p => p.Seller).Include(p=> p.Customer).Include(p => p.Product).ToList();
    }
}
