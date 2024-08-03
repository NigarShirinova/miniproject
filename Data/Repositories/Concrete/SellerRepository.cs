using Core.Entities;
using Data.Context;
using Data.Repositories.Abstract;
using Data.Repositories.Base;

namespace Data.Repositories.Concrete;
public class SellerRepository : BaseRepository<Seller>, ISellerRepository
{
    public SellerRepository(AppDbContext context) : base(context)
    {
    }
}
