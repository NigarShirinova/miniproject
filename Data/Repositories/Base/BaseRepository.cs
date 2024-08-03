using Core.Constants;
using Core.Entities.Base;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Base;
public class BaseRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbTable;
    public BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbTable = _context.Set<T>();
    }
    public void Add(T item)
    {
        item.CreatedDate = DateTime.Now;
        _dbTable.Add(item);
    }

    public void Delete(T item)
    {
        if (item is BaseEntity entity)
        {
            entity.IsDeleted = true;
        }
        else
        {
            Messages.ErrorOccuredMessage();
        }
    }

    public T Get(int id)
    {
        return _dbTable.Find(id);
    }

    public virtual List<T> GetAll()
    {
        return _dbTable.ToList<T>();
    }

    public void Update(T item)
    {
        item.UpdatedDate = DateTime.Now;
        _dbTable.Update(item);
    }


}

