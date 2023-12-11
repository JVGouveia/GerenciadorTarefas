using System.Collections.Generic;
using ConsoleTables;
using TestePoo.Data;
using TestePoo.Interfaces;
using TestePoo.Models;
using TestePoo.Repositories;

namespace TestePoo.Services
{
    public class TarefaService
    {
        private readonly TarefaRepository _tarefaRepository;
        private readonly ListaRepository _listaRepository;

        
        public TarefaService(TarefaRepository tarefaRepository, ListaRepository listaRepository)
        {
            _tarefaRepository = tarefaRepository;
            _listaRepository = listaRepository;
        }

        public void Add(DataContext? context, Usuario usuario)
        {
            string nome, descricao;
            DateTime dataLimite = default;

            Console.WriteLine("Informe o nome da tarefa:");
            nome = Console.ReadLine();

            Console.WriteLine("Informe a descrição da tarefa (pode ser vazia):");
            descricao = Console.ReadLine();

            while (true)
            {
                Console.WriteLine("Informe a data limite da tarefa no formato dia/mes/ano (pode ser vazia):");
                string inputDataLimite = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(inputDataLimite))
                {
                    break;
                }

                if (DateTime.TryParse(inputDataLimite, out DateTime result))
                {
                    DateTime dataAtual = DateTime.Now;

                    // Verifica se a data é anterior à data atual
                    if (result < dataAtual)
                    {
                        Console.WriteLine("A data não pode ser anterior à data atual. Digite novamente.");
                    }
                    // Verifica se a data é mais de um ano após a data atual
                    else if (result > dataAtual.AddYears(1))
                    {
                        Console.WriteLine("A data não pode ser mais de um ano após a data atual. Digite novamente.");
                    }
                    else
                    {
                        dataLimite = result;
                        break; 
                    }
                }
                else
                {
                    Console.WriteLine("Formato de data inválido! Digite novamente.");
                }
            }

            int listaId = _listaRepository.EscolherLista(new ListaService(_listaRepository), usuario);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Tarefa tarefa = new Tarefa(nome, descricao, dataLimite, listaId);
                    _tarefaRepository.Add(tarefa);
                    transaction.Commit();
                    Console.Clear();
                    Console.WriteLine("Tarefa adicionada com sucesso");
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
            Console.Clear();
            Console.WriteLine("Tarefa excluida com sucesso");
        }
        
        public List<Tarefa> GetTarefasPorListas(List<Lista> listas)
        {
            return _tarefaRepository.GetTarefasPorListas(listas);
        }

        public int EscolherTarefa(TarefaService tarefaService, ListaService listaService, Usuario usuario)
        {
            return _tarefaRepository.EscolherTarefa(tarefaService, listaService, usuario);
        }
        
        public void UpdateTarefa(DataContext? context, TarefaService tarefaService, ListaService listaService, Usuario usuario)
    {
        var tarefaId = tarefaService.EscolherTarefa(tarefaService, listaService, usuario);

        Console.Write("Informe o nome (deixe vazio caso não deseje alterar):");
        var nome = Console.ReadLine();

        Console.Write("Informe a descrição (deixe vazio caso não deseje alterar):");
        var descricao = Console.ReadLine();

        DateTime? dataLimite = null;
        Console.Write("Informe a data limite (opcional - formato dd/MM/yyyy): ");
        var dataLimiteStr = Console.ReadLine();

        if (!string.IsNullOrEmpty(dataLimiteStr))
        {
            if (DateTime.TryParseExact(dataLimiteStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None,
                    out var parsedDataLimite))
            {
                DateTime dataAtual = DateTime.Now;

                // Verificar se a data é anterior à data atual
                if (parsedDataLimite < dataAtual)
                {
                    Console.WriteLine("A data não pode ser anterior à data atual. A data não será alterada.");
                }
                // Verificar se a data é mais de um ano após a data atual
                else if (parsedDataLimite > dataAtual.AddYears(1))
                {
                    Console.WriteLine("A data não pode ser mais de um ano após a data atual. A data não será alterada.");
                }
                else
                {
                    dataLimite = parsedDataLimite;
                }
            }
            else
            {
                Console.WriteLine("Formato de data inválido. A data não será alterada.");
            }
        }

        Console.Write("Informe o status (deixe vazio caso não deseje alterar - Pendente ou Concluida):");
        var status = Console.ReadLine();
        while (status.ToLower() != "pendente" && status.ToLower() != "concluida" && !string.IsNullOrWhiteSpace(status))
        {
            Console.WriteLine(status.ToLower());
            Console.Write("Status inválido, Informe novamente: ");
            status = Console.ReadLine();
        }

        var listaId = listaService.EscolherLista(listaService, usuario);

        using (var transaction = context.Database.BeginTransaction())
        {
            try
            {
                tarefaService.Update(new Tarefa(tarefaId,
                    string.IsNullOrEmpty(nome) ? tarefaService.GetById(tarefaId).Nome : nome,
                    string.IsNullOrEmpty(descricao) ? tarefaService.GetById(tarefaId).Descricao : descricao,
                    dataLimite ?? tarefaService.GetById(tarefaId).DataLimite,
                    (string.IsNullOrEmpty(status)
                        ? tarefaService.GetById(tarefaId).Status
                        : (status.ToLower() == "pendente" ? 0 : 1)),
                    listaId));
                transaction.Commit();
                Console.Clear();
                Console.Write("Tarefa alterada com sucesso");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao atualizar a tarefa: {e.Message}");
                transaction.Rollback();
                throw;
            }
        }
    }
    }
}