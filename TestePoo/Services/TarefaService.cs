using ConsoleTables;
using Spectre.Console;
using TestePoo.Data;
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
            var listasNoBanco = _listaRepository.GetListasPorUsuario(usuario.UsuarioId);
            
            if (listasNoBanco == null || !listasNoBanco.Any())
            {
                Console.Clear();
                Console.WriteLine("Você nao possui nenhuma lista. Crie uma lista antes de adicionar uma tarefa.");
                return;
            }
        
            string nome, descricao;
            DateTime dataLimite = default;
        
            Console.WriteLine("Informe o nome da tarefa: ");
            nome = Console.ReadLine();
        
            Console.WriteLine("Informe a descrição da tarefa (pode ser vazia): ");
            descricao = Console.ReadLine();
        
            while (true)
            {
                Console.WriteLine("Informe a data limite da tarefa no formato dia/mes/ano (pode ser vazia): ");
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
                        Console.WriteLine("A data não pode ser anterior à data atual. Digite novamente: ");
                    }
                    // Verifica se a data é mais de um ano após a data atual
                    else if (result > dataAtual.AddYears(1))
                    {
                        Console.WriteLine("A data não pode ser mais de um ano após a data atual. Digite novamente: ");
                    }
                    else
                    {
                        dataLimite = result;
                        break; 
                    }
                }
                else
                {
                    Console.WriteLine("Formato de data inválido! Digite novamente: ");
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
                    Console.WriteLine("Tarefa adicionada com sucesso!");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao criar a tarefa: {e.Message}");
                    transaction.Rollback();
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

        public void Delete(TarefaService tarefaService, ListaService listaService, Usuario usuario)
        {
            
            var tarefasDaLista = GetAllTarefas(tarefaService, listaService, usuario);

            if (!tarefasDaLista.Any())
            {
                Console.Clear();
                Console.WriteLine("Não há tarefas para excluir.");
                return;
            }
            
            int TarefaId = EscolherTarefa(tarefaService, listaService, usuario);
            
            Tarefa tarefaParaExcluir = GetById(TarefaId);
            try{
                if (tarefaParaExcluir != null && _listaRepository.GetById(tarefaParaExcluir.ListaId).UsuarioId == usuario.UsuarioId)
                {
                    _tarefaRepository.Delete(TarefaId);
                    Console.Clear();
                    Console.WriteLine("Tarefa excluída com sucesso!");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Você não tem permissão para excluir esta tarefa ou a tarefa não existe.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir a tarefa: {ex.Message}");
            }
        }

        
        public List<Tarefa>? GetTarefasPorListas(List<Lista> listas)
        {
            return _tarefaRepository.GetTarefasPorListas(listas);
        }
        
        public List<Tarefa> GetTarefasPorLista(Lista lista)
        {
            return _tarefaRepository.GetTarefasPorLista(lista);
        }

        public int EscolherTarefa(TarefaService tarefaService, ListaService listaService, Usuario usuario)
        {
            return _tarefaRepository.EscolherTarefa(tarefaService, listaService, usuario);
        }
        
        public void UpdateTarefa(DataContext? context, TarefaService tarefaService, ListaService listaService, Usuario usuario)
        {
            
            var tarefasDaLista = GetAllTarefas(tarefaService, listaService, usuario);

            if (!tarefasDaLista.Any())
            {
                Console.Clear();
                Console.WriteLine("Não há tarefas para modificar.");
                return;
            }
            
            var tarefaId = EscolherTarefa(tarefaService, listaService, usuario);
    
            var usuarioDaTarefa = listaService.GetById(GetById(tarefaId).ListaId).UsuarioId; 
    
            if (usuarioDaTarefa != usuario.UsuarioId)
            {
                Console.Clear();
                Console.WriteLine("Você não tem permissão para atualizar esta tarefa.");
                return;
            }
            
            Console.WriteLine("Informe o nome (deixe vazio caso não deseje alterar): ");
            var nome = Console.ReadLine();
    
            Console.WriteLine("Informe a descrição (deixe vazio caso não deseje alterar): ");
            var descricao = Console.ReadLine();
    
            DateTime? dataLimite = null;
            Console.WriteLine("Informe a data limite (opcional - formato dd/MM/yyyy): ");
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
    
            Console.WriteLine("Informe o status (deixe vazio caso não deseje alterar - Pendente ou Concluida): ");
            var status = Console.ReadLine();
            while (status.ToLower() != "pendente" && status.ToLower() != "concluida" && !string.IsNullOrWhiteSpace(status))
            {
                Console.WriteLine("Status inválido, Informe novamente: ");
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
                    Console.WriteLine("Tarefa alterada com sucesso");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao atualizar a tarefa: {e.Message}");
                    transaction.Rollback();
                }
            }
        }
        public List<Tarefa> GetAllTarefas(TarefaService tarefaService, ListaService listaService, Usuario usuario)
        {
            var listas = listaService.GetListasPorUsuario(usuario.UsuarioId);
            var tarefasDaLista = tarefaService.GetTarefasPorListas(listas);

            if (tarefasDaLista.Any())
            {
                Tree root = new Tree("Tarefas");

                foreach (var lista in listas)
                {
                    var tarefasDaListaAtual = tarefasDaLista.Where(t => t.ListaId == lista.ListaId).ToList();

                    if (tarefasDaListaAtual.Any())
                    {
                        var l = root.AddNode($"{lista.Nome}");

                        var table = new Table()
                            .RoundedBorder()
                            .AddColumn("ID")
                            .AddColumn("Nome")
                            .AddColumn("Status")
                            .AddColumn("Data limite");

                        foreach (var tarefa in tarefasDaListaAtual)
                        {
                            table.AddRow($"{tarefa.TarefaId.ToString()}", $"{tarefa.Nome}", $"{(tarefa.Status == 0 ? "Pendente" : "Concluida")}", $"{tarefa.DataLimite.ToString("dd/MM/yyyy")}");
                        }
                        l.AddNode(table);
                    }
                }

                AnsiConsole.Write(root);

                return tarefasDaLista.ToList();
            }
            return new List<Tarefa>();
        }

        
        public void GetTarefasPorLista(TarefaService tarefaService, ListaService listaService, Usuario usuario)
        {
            
            var listasNoBanco = _listaRepository.GetListasPorUsuario(usuario.UsuarioId);
            
            if (listasNoBanco == null || !listasNoBanco.Any())
            {
                Console.Clear();
                Console.WriteLine("Você nao possui nenhuma lista. Crie uma lista antes.");
                return;
            }
            
            Console.WriteLine("Selecione uma lista para exibir as tarefas: ");
            var lista = listaService.GetById(listaService.EscolherLista(listaService, usuario));
            
            Console.WriteLine($"Tarefas da Lista '{lista.Nome}':");

            // Obter todas as tarefas associadas à lista
            var tarefasDaLista = GetTarefasPorLista(lista);
            
            
            if (tarefasDaLista.Any())
            {
                var table = new Table()
                    .RoundedBorder()
                    .AddColumn("ID")
                    .AddColumn("Nome")
                    .AddColumn("Status")
                    .AddColumn("Data limite");

                foreach (var tarefa in tarefasDaLista)
                {
                    table.AddRow($"{tarefa.TarefaId.ToString()}", $"{tarefa.Nome}", $"{(tarefa.Status == 0 ? "Pendente" : "Concluida")}", $"{tarefa.DataLimite.ToString("dd/MM/yyyy")}");
                }
                
                AnsiConsole.Write(table);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Não há tarefas nesta lista.");
            }
        }
        
        public void AlterarStatusEmLote(TarefaService tarefaService, ListaService listaService, Usuario usuario, DataContext context)
        {
            // Obtenha as tarefas do serviço
            var tarefas = GetTarefasPorListas(listaService.GetListasPorUsuario(usuario.UsuarioId));

            var table = new Table();
            table.AddColumn("ID");
            table.AddColumn("Nome");
            table.AddColumn("Lista");

            foreach (var tarefa in tarefas)
            {
                table.AddRow(tarefa.TarefaId.ToString(), tarefa.Nome, listaService.GetById(tarefa.ListaId).Nome);
            }

            AnsiConsole.Write(table);

            // Obter seleções múltiplas do usuário
            var multiSelectionPrompt = new MultiSelectionPrompt<int>()
                .Title("Selecione os itens desejados (espaço para selecionar, Enter para confirmar)")
                .MoreChoicesText("Existem mais itens...")!;
            
            foreach (var tarefa in tarefas)
            {
                multiSelectionPrompt.AddChoices(tarefa.TarefaId);
            }
            
            var selection = AnsiConsole.Prompt(
                multiSelectionPrompt
            );

            foreach (var indice in selection)
            {
                Console.WriteLine(indice);
            }
            
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var indice in selection)
                    {
                        tarefaService.Update(new Tarefa(indice,
                            GetById(indice).Nome,
                            GetById(indice).Descricao,
                            GetById(indice).DataLimite,
                            1,
                            GetById(indice).ListaId));
                    }
                    transaction.Commit();
                    Console.Clear();
                    Console.WriteLine("Status das tarefas atualizado em lote!");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao alterar status em lote: {e.Message}");
                    transaction.Rollback();
                }
            }
        }
    } 
}