using Microsoft.EntityFrameworkCore;
using TestePoo.Models;

namespace TestePoo.Repositories;

public class ListaRepository : Repository<Lista>
{
    public ListaRepository(DbContext context) : base(context)
    {
    }
}