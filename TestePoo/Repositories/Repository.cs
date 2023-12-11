using Microsoft.EntityFrameworkCore;
using TestePoo.Interfaces;

namespace TestePoo.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected static DbContext? Context { get; set; }
    private readonly DbSet<T> _dbSet;

    public Repository(DbContext? context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = Context.Set<T>();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
        Context.SaveChanges();
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
        Context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = GetById(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            Context.SaveChanges();
        }
    }
    
    public T ExisteNaBaseDeDados(string tabela, string coluna, string dado)
    {
        var a = Context.Set<T>()
            .FromSqlRaw($"SELECT * FROM {tabela} Where {coluna} =  '{dado}'")
            .ToList().FirstOrDefault();

        return a;
    }
}
