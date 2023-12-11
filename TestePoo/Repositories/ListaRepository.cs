using ConsoleTables;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
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
        var listas = listaService.GetListasPorUsuario(usuario.UsuarioId);
        
        Console.WriteLine("\nListas:");

        var selection = AnsiConsole.Prompt(new SelectionPrompt<Lista>()
            .Title("Escolha uma lista")
            .PageSize(5)
            .AddChoices(listas)
            .UseConverter(lista => $"{lista.ListaId}: {lista.Nome}"));

        return selection.ListaId;
    }
}