using System.Collections.Generic;
using ConsoleTables;
using TestePoo.Data;
using TestePoo.Interfaces;
using TestePoo.Models;

namespace TestePoo.Services
{
    public class TarefaService
    {
        private readonly IRepository<Tarefa> _tarefaRepository;
        private readonly IRepository<Lista> _listaRepository;

        public TarefaService(IRepository<Tarefa> tarefaRepository, IRepository<Lista> listaRepository)
        {
            _tarefaRepository = tarefaRepository;
            _listaRepository = listaRepository;
        }

        public void Add(DataContext context)
        {
            string nome, descricao;
            DateTime dataLimite;

            Console.WriteLine("Informe o nome da tarefa:");
            nome = Console.ReadLine();

            Console.WriteLine("Informe a descrição da tarefa (pode ser vazia):");
            descricao = Console.ReadLine();

            while (true)
            {
                Console.WriteLine("Informe a data limite da tarefa no formato dia/mes/ano (pode ser vazia):");
                string inputDataLimite = Console.ReadLine();

                if (DateTime.TryParse(inputDataLimite, out DateTime result))
                {
                    dataLimite = result;
                    break;
                }

                Console.WriteLine("Formato de data inválido! Digite novamente.");
            }

            Console.WriteLine("\nListas:");
            var table = new ConsoleTable("Id", "Nome");

            foreach (var lista in _listaRepository.GetAll())
            {
                table.AddRow($"{lista.ListaId}", $"{lista.Nome}");
            }
            table.Configure(o => o.EnableCount = false)
                .Write(Format.Minimal);

            int listaId;
            bool idValido = false;

            do
            {
                Console.Write("Informe o Id da lista:");

                if (int.TryParse(Console.ReadLine(), out listaId))
                {
                    idValido = _listaRepository.GetAll().Any(lista => lista.ListaId == listaId);

                    if (!idValido)
                        Console.WriteLine("Por favor, informe um Id válido da lista.");
                }
            } while (!idValido);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Tarefa tarefa = new Tarefa(nome, descricao, dataLimite, listaId);
                    _tarefaRepository.Add(tarefa);
                    transaction.Commit();
                    // return tarefa;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao criar a tarefa: {e.Message}");
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public Tarefa GetById(int id)
        {
            return _tarefaRepository.GetById(id);
        }

        public IEnumerable<Tarefa> GetAll()
        {
            return _tarefaRepository.GetAll();
        }

        public void Update(Tarefa tarefa)
        {
            _tarefaRepository.Update(tarefa);
        }

        public void Delete(int id)
        {
            _tarefaRepository.Delete(id);
        }
    }
}