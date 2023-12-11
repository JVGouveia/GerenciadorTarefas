using ConsoleTables;
using Microsoft.EntityFrameworkCore;
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
        public List<Tarefa> GetTarefasPorListas(List<Lista> listas)
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
            Console.WriteLine("\nTarefas:");
            var table = new ConsoleTable("Id", "Nome");

            foreach (var tarefa in tarefaService.GetTarefasPorListas(listaService.GetListasPorUsuario(usuario.UsuarioId)))
            {
                table.AddRow($"{tarefa.TarefaId.ToString()}", $"{tarefa.Nome}");
            }

            table.Configure(o => o.EnableCount = false)
                .Write(Format.Minimal);

            int TarefaId;
            bool idValido = false;

            do
            {
                Console.Write("Informe o Id da tarefa:");

                if (int.TryParse(Console.ReadLine(), out TarefaId))
                {
                    idValido = tarefaService.GetAll().Any(tarefa => tarefa.TarefaId == TarefaId);

                    if (!idValido)
                        Console.WriteLine("Por favor, informe um Id válido.");
                }
            } while (!idValido);

            return TarefaId;
        }
    }
}