using Core.Constants;
using Data.Context;
using Data.Repositories.Concrete;
using Data.UnitOfWork.Abstract;

namespace Data.UnitOfWork.Concrete;
public class UnitOfWork : IUnitOfWork
{
    public readonly OrderRepository Orders;
    public readonly SellerRepository Sellers;
    public readonly CustomerRepository Customers;
    public readonly AdminRepository Admins;
    public readonly CategoryRepository Categories;
    public readonly ProductRepository Products;
    private readonly AppDbContext _context;

    public UnitOfWork()
    {
        _context = new AppDbContext();
        Sellers = new SellerRepository(_context);
        Customers = new CustomerRepository(_context);
        Admins = new AdminRepository(_context);
        Categories = new CategoryRepository(_context);
        Products = new ProductRepository(_context);
        Orders = new OrderRepository(_context);
    }

    public void Commit()
    {
        try
        {
            _context.SaveChanges();
            Messages.SuccessMessage();
        }
        catch (Exception ex)
        {
            Messages.ErrorOccuredMessage();
        }

    }
    public void CommitWithoutMessage()
    {
        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            Messages.ErrorOccuredMessage();
        }

    }
}
