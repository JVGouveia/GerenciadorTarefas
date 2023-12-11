using ConsoleTables;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using TestePoo.Data;
using TestePoo.Models;
using TestePoo.Services;

namespace TestePoo.Repositories
{
    public class TarefaRepository : Repository<Tarefa>
    {
        public TarefaRepository(DataContext? context) : base(context)
        {
        }
        public List<Tarefa>? GetTarefasPorListas(List<Lista> listas)
        {
            var listaIds = listas.Select(lista => lista.ListaId);
    
            return Context.Set<Tarefa>()
                .Where(tarefa => listaIds.Contains(tarefa.ListaId))
                .ToList();
        }
        
        public List<Tarefa> GetTarefasPorLista(Lista lista)
        {
            return Context.Set<Tarefa>()
                .Where(tarefa => tarefa.ListaId == lista.ListaId)
                .ToList();
        }

        public void Update(Tarefa tarefa)
        {
            var tarefaExistente = GetById(tarefa.TarefaId);

            if (tarefaExistente == null)
            {
                Console.WriteLine($"Tarefa com ID {tarefa.TarefaId} não encontrada.");
                return;
            }

            tarefaExistente.Nome = tarefa.Nome;
            tarefaExistente.Descricao = tarefa.Descricao;
            tarefaExistente.DataLimite = tarefa.DataLimite;
            tarefaExistente.Status = tarefa.Status;
            tarefaExistente.ListaId = tarefa.ListaId;

            Context?.SaveChanges();

            Console.WriteLine("Tarefa atualizada com sucesso!");
            Console.Clear();
        }

        public int EscolherTarefa(TarefaService tarefaService, ListaService listaService, Usuario usuario)
        {
            var listas = listaService.GetListasPorUsuario(usuario.UsuarioId);

            Console.WriteLine("\nTarefas:");

            var selection = AnsiConsole.Prompt(new SelectionPrompt<Tarefa>()
                .Title("Escolha uma tarefa")
                .PageSize(5)
                .AddChoices(tarefaService.GetTarefasPorListas(listas))
                .UseConverter(tarefa => $"{tarefa.TarefaId}: {tarefa.Nome}"));

            return selection.TarefaId;
        }
    }
}