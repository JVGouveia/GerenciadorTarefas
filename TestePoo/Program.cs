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

                var tarefaService = new TarefaService(new Repository<Tarefa>(context), new Repository<Lista>(context));
                var usuarioService = new UsuarioService(new Repository<Usuario>(context));
                var listaService = new ListaService(new Repository<Lista>(context));

                while (true)
                {
                    Console.WriteLine("\nMenu Principal:");
                    Console.WriteLine("1. Já possuo cadastro");
                    Console.WriteLine("2. Quero me cadastrar");
                    Console.WriteLine("3. Sair");

                    Console.Write("Escolha uma opção: ");
                    string escolha = Console.ReadLine();

                    switch (escolha)
                    {
                        case "1":
                            usuarioAtual = usuarioService.ValidarUsuario(context);
                            if (usuarioAtual != null)
                            {
                                Console.WriteLine("Login bem-sucedido!");
                                while(usuarioAtual != null)
                                    MenuLogado();
                            }
                            else
                            {
                                Console.WriteLine("Usuário não encontrado ou senha incorreta.");
                            }
                            break;

                        case "2":
                            usuarioAtual = usuarioService.Add(context);
                            Console.WriteLine("Cadastro bem-sucedido! Agora você está logado.");
                            MenuLogado();
                            break;

                        case "3":
                            // Opção para sair
                            Console.WriteLine("Você escolheu: Sair");
                            Environment.Exit(0); // Encerra o programa
                            break;

                        default:
                            Console.WriteLine("Opção inválida. Por favor, escolha uma opção válida.");
                            break;
                    }
                }
            }
        }
        static void MenuLogado()
        {
            Console.WriteLine("\nMenu Logado:");
            Console.WriteLine("1. Visualizar Perfil");
            Console.WriteLine("2. Fazer Logout");

            Console.Write("Escolha uma opção: ");
            string escolha = Console.ReadLine();

            switch (escolha)
            {
                case "1":
                    // Implemente a lógica para visualizar o perfil do usuário
                    Console.WriteLine($"Perfil do usuário {usuarioAtual.Nome}");
                    break;

                case "2":
                    // Implemente a lógica para fazer logout
                    usuarioAtual = null;
                    Console.WriteLine("Logout bem-sucedido!");
                    break;

                default:
                    Console.WriteLine("Opção inválida. Por favor, escolha uma opção válida.");
                    break;
            }
        }
    }
}
