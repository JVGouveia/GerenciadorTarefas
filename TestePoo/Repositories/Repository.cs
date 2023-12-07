using Microsoft.EntityFrameworkCore;
using TestePoo.Interfaces;

namespace TestePoo.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public T GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = GetById(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
    
    public T ExisteNaBaseDeDados(string tabela, string coluna, string dado)
    {
        var a = _context.Set<T>()
            .FromSqlRaw($"SELECT * FROM {tabela} Where {coluna} =  '{dado}'")
            .ToList().FirstOrDefault();

        return a;
    }
}
