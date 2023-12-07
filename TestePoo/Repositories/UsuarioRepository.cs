using Microsoft.EntityFrameworkCore;
using TestePoo.Models;

namespace TestePoo.Repositories;

public class UsuarioRepository : Repository<Usuario>
{
    public UsuarioRepository(DbContext context) : base(context)
    {
    }
    
    
}