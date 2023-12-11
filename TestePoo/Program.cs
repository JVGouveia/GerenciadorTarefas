using ConsoleTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestePoo.Data;
using TestePoo.Interfaces;
using TestePoo.Models;
using TestePoo.Repositories;
using TestePoo.Services;

namespace TestePoo
{
    class Program
    {
        static Usuario usuarioAtual = null;
        
        static void Main(string[] args)
        {
            using (var context = new DataContext())
            {
                context.Database.EnsureCreated();

                var tarefaService = new TarefaService(new TarefaRepository(context), new ListaRepository(context));
                var usuarioService = new UsuarioService(new UsuarioRepository(context));
                var listaService = new ListaService(new ListaRepository(context));

                while (true)
                {
                    Console.WriteLine("=== Menu Principal ===");
                    Console.WriteLine("1. Já possuo cadastro");
                    Console.WriteLine("2. Quero me cadastrar");
                    Console.WriteLine("3. Sair");
                    
                    switch (LerOpcao())
                    {
                        case 1:
                            usuarioAtual = usuarioService.ValidarUsuario(context);
                            if (usuarioAtual != null)
                            {
                                MenuUsuario(tarefaService, listaService, usuarioService, context);
                            }
                            break;
                        case 2:
                            usuarioService.Add(context);
                            Console.WriteLine("Cadastro realizado com sucesso!");
                            break;
                        case 3:
                            Console.WriteLine("Saindo do sistema. Até logo!");
                            return;
                        default:
                            Console.WriteLine("Opção inválida. Tente novamente.");
                            break;
                    }
                }
            }
        }
        static void MenuUsuario(TarefaService tarefaService, ListaService listaService, UsuarioService usuarioService, DataContext? context)
    {
        while (true)
        {
            Console.WriteLine($"\n=== Menu do Usuário ===");
            Console.WriteLine("1. Operações em Tarefas");
            Console.WriteLine("2. Operações em Listas");
            Console.WriteLine("3. Visualizar informações do usuário");
            Console.WriteLine("4. Sair");
            
            switch (LerOpcao())
            {
                case 1:
                    MenuTarefas(tarefaService, listaService, context);
                    break;
                case 2:
                    MenuListas();
                    break;
                case 3:
                    Console.WriteLine($"ID: {usuarioAtual.UsuarioId}, Nome: {usuarioAtual.Nome}, Email: {usuarioAtual.Email}");
                    break;
                case 4:
                    Console.WriteLine("Saindo do menu do usuário.");
                    return;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }

    static void MenuTarefas(TarefaService tarefaService, ListaService listaService, DataContext? context)
    {
        while (true)
        {
            Console.WriteLine("\n=== Menu de Tarefas ===");
            Console.WriteLine("1. Adicionar Tarefa");
            Console.WriteLine("2. Atualizar Tarefa");
            Console.WriteLine("3. Excluir Tarefa");
            Console.WriteLine("4. Buscar todas Tarefas");
            Console.WriteLine("5. Buscar Tarefa por Lista");
            Console.WriteLine("6. Buscar Tarefa por Usuário");
            Console.WriteLine("7. Voltar ao Menu do Usuário");
            
            switch (LerOpcao())
            {
                case 1:
                    tarefaService.Add(context, usuarioAtual);
                    break;
                case 2:
                    tarefaService.UpdateTarefa(context,tarefaService, listaService, usuarioAtual);
                    break;
                case 3:
                    Console.WriteLine("\nTarefas:");
                    var table = new ConsoleTable("Id", "Nome");

                    foreach (var tarefa in tarefaService.GetTarefasPorListas(listaService.GetListasPorUsuario(usuarioAtual.UsuarioId)))
                    {
                        table.AddRow($"{tarefa.TarefaId.ToString()}", $"{tarefa.Nome}");
                    }
                    table.Configure(o => o.EnableCount = false)
                        .Write(Format.Minimal);

                    int TarefaId;
                    bool idValido = false;

                    do
                    {
                        Console.Write("Informe o Id da lista:");

                        if (int.TryParse(Console.ReadLine(), out TarefaId))
                        {
                            idValido = tarefaService.GetAll().Any(tarefa => tarefa.TarefaId == TarefaId);

                            if (!idValido)
                                Console.WriteLine("Por favor, informe um Id válido.");
                        }
                    } while (!idValido);
                    
                    tarefaService.Delete(TarefaId);
                    break;
                case 4:
                    Console.WriteLine("\nTarefas:");
                    var tarefas = new ConsoleTable("Id", "Nome", "Status");

                    foreach (var tarefa in tarefaService.GetTarefasPorListas(listaService.GetListasPorUsuario(usuarioAtual.UsuarioId)))
                    {
                        tarefas.AddRow($"{tarefa.TarefaId.ToString()}", $"{tarefa.Nome}", $"{(tarefa.Status == 0 ? "Pendente" : "Concluida")}", $"{listaService.GetById(tarefa.TarefaId).Nome}");
                    }
                    tarefas.Configure(o => o.EnableCount = false)
                        .Write(Format.Minimal);                  
                    break;
                case 5:
                    // Implementar lógica para buscar tarefa por lista
                    break;
                case 6:
                    // Implementar lógica para buscar tarefa por usuário
                    break;
                case 7:
                    Console.WriteLine("Voltando ao Menu do Usuário.");
                    return;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }

    static void MenuListas()
    {
        while (true)
        {
            Console.WriteLine("\n=== Menu de Listas ===");
            Console.WriteLine("1. Adicionar Lista");
            Console.WriteLine("2. Atualizar Lista");
            Console.WriteLine("3. Excluir Lista");
            Console.WriteLine("4. Buscar Tarefas por Lista");
            Console.WriteLine("5. Buscar Listas por Usuário");
            Console.WriteLine("6. Voltar ao Menu do Usuário");

            int opcao = LerOpcao();

            switch (opcao)
            {
                case 1:
                    // Implementar lógica para adicionar tarefa
                    break;
                case 2:
                    // Implementar lógica para atualizar tarefa
                    break;
                case 3:
                    // Implementar lógica para excluir tarefa
                    break;
                case 4:
                    // Implementar lógica para buscar tarefa por lista
                    break;
                case 5:
                    // Implementar lógica para buscar tarefa por usuário
                    break;
                case 6:
                    Console.WriteLine("Voltando ao Menu do Usuário.");
                    return;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }

    // Implementar MenuListas de forma análoga ao MenuTarefas

    static int LerOpcao()
    {
        Console.Write("Escolha uma opção: ");
        int opcao;

        while (!int.TryParse(Console.ReadLine(), out opcao))
        {
            Console.Write("Opção inválida. Tente novamente: ");
        }

        return opcao;
    }
    }
}
