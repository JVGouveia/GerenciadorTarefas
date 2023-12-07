using Microsoft.EntityFrameworkCore;
using TestePoo.Models;

namespace TestePoo.Repositories;

public class TarefaRepository : Repository<Usuario>
{
    public TarefaRepository(DbContext context) : base(context)
    {
    }
}