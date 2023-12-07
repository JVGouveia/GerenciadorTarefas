namespace TestePoo.Interfaces;

public interface IRepository<T>
{
    void Add(T entity);

    T GetById(int id);
    IEnumerable<T> GetAll();

    void Update(T entity);

    void Delete(int id);

    public T ExisteNaBaseDeDados(string tabela, string coluna, string dado);
}
