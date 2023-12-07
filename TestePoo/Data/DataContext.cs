using Microsoft.EntityFrameworkCore;
using TestePoo.Models;

namespace TestePoo.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Usuario>? Usuarios { get; set; }
        public DbSet<Lista>? Listas { get; set; }
        public DbSet<Tarefa>? Tarefas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var serverVersion = ServerVersion.AutoDetect("server=localhost;user=root;password=1234;database=tarefas");
            optionsBuilder.UseMySql("server=localhost;user=root;password=1234;database=tarefas", serverVersion);
        }

    }
}