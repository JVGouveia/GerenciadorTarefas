using ConsoleTables;
using Microsoft.EntityFrameworkCore;
using TestePoo.Models;
using TestePoo.Services;

namespace TestePoo.Repositories;

public class ListaRepository : Repository<Lista>
{
    public ListaRepository(DbContext? context) : base(context)
    {
    }
    public List<Lista> GetListasPorUsuario(int usuarioId)
    {
        return Context.Set<Lista>()
            .Where(lista => lista.UsuarioId == usuarioId)
            .ToList();
    }
    
    public int EscolherLista(ListaService listaService, Usuario usuario)
    {
        Console.WriteLine("\nListas:");
        var table = new ConsoleTable("Id", "Nome");

        foreach (var x in listaService.GetListasPorUsuario(usuario.UsuarioId))
        {
            table.AddRow($"{x.ListaId.ToString()}", $"{x.Nome}");
        }

        table.Configure(o => o.EnableCount = false)
            .Write(Format.Minimal);

        int ListaId;
        bool idValido = false;

        do
        {
            Console.Write("Informe o Id da lista:");

            if (int.TryParse(Console.ReadLine(), out ListaId))
            {
                idValido = listaService.GetAll().Any(l => l.ListaId == ListaId);

                if (!idValido)
                    Console.WriteLine("Por favor, informe um Id válido.");
            }
        } while (!idValido);

        return ListaId;
    }
}