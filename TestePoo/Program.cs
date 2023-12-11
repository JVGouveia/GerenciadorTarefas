using TestePoo.Data;
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
                    MenuListas(listaService, context);
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
            Console.WriteLine("2. Alterar Tarefa");
            Console.WriteLine("3. Excluir tarefa");
            Console.WriteLine("4. Buscar tarefas");
            Console.WriteLine("5. Buscar tarefas por lista");
            Console.WriteLine("6. Voltar ao Menu do Usuário");
            
            switch (LerOpcao())
            {
                case 1:
                    tarefaService.Add(context, usuarioAtual);
                    break;
                case 2:
                    tarefaService.UpdateTarefa(context,tarefaService, listaService, usuarioAtual);
                    break;
                case 3:
                    tarefaService.Delete(tarefaService, listaService, usuarioAtual);
                    break;
                case 4:
                    tarefaService.GetAllTarefas(tarefaService, listaService, usuarioAtual);
                    break;
                case 5:
                    tarefaService.GetTarefasPorLista(tarefaService, listaService, usuarioAtual);
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

    static void MenuListas(ListaService listaService, DataContext? context)
    {
        while (true)
        {
            Console.WriteLine("\n=== Menu de Listas ===");
            Console.WriteLine("1. Adicionar Lista");
            Console.WriteLine("2. Alterar lista");
            Console.WriteLine("3. Excluir lista");
            Console.WriteLine("4. Buscar listas");
            Console.WriteLine("5. Voltar ao Menu do Usuário");
            
            switch (LerOpcao())
            {
                case 1:
                    listaService.Add(context, usuarioAtual);
                    break;
                case 2:
                    listaService.UpdateLista(context, listaService, usuarioAtual);
                    break;
                case 3:
                    listaService.Delete(listaService, usuarioAtual);
                    break;
                case 4:
                    listaService.GetAllListas(listaService, usuarioAtual);
                    break;
                case 5:
                    Console.WriteLine("Voltando ao Menu do Usuário.");
                    return;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }
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
