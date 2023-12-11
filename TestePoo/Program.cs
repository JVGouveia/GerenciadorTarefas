using Spectre.Console;
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
            try
            {
                Console.BackgroundColor = Color.Black;
                Console.Clear();
                
                using (var context = new DataContext())
                {
                    context.Database.EnsureCreated();
    
                    var tarefaService = new TarefaService(new TarefaRepository(context), new ListaRepository(context));
                    var usuarioService = new UsuarioService(new UsuarioRepository(context));
                    var listaService = new ListaService(new ListaRepository(context));
    
                    while (true)
                    {
                        var opcao = AnsiConsole.Prompt(
                            new SelectionPrompt<(int Index, string Name)>()
                               .AddChoices(new List<(int Index, string Name)>
                               {
                                   (0, "Sair"),
                                   (1, "Já possuo cadastro"),
                                   (2, "Quero me cadastrar"),
                               })
                               .UseConverter(a => a.Name)
                               .HighlightStyle(new Style().Background(Color.DarkBlue))
                               .Title("=== Menu Principal ===")
                        );
                        switch(opcao.Index)
                        {
                            case 0: 
                                return;
                            case 1:
                                usuarioAtual = usuarioService.ValidarUsuario(context);
                                if (usuarioAtual != null)
                                {
                                    Console.Clear();
                                    MenuUsuario(tarefaService, listaService, usuarioService, context);
                                };
                                break;
                            case 2:
                                usuarioAtual = usuarioService.Add(context);
                                Console.WriteLine("Cadastro realizado com sucesso!");
                                if (usuarioAtual != null)
                                {
                                    Console.Clear();
                                    MenuUsuario(tarefaService, listaService, usuarioService, context);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
            {
                Console.WriteLine("Não é possivel modificar esse dado.");
            }
            catch (MySqlConnector.MySqlException e)
            {
                Console.WriteLine("Erro de SQL.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro não esperado: {ex.Message}");
            }
        }
        static void MenuUsuario(TarefaService tarefaService, ListaService listaService, UsuarioService usuarioService, DataContext? context)
    {
        while (true)
        {
            var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<(int Index, string Name)>()
                    .AddChoices( new List<(int Index, string Name)>
                    {
                        (0, "Voltar"),
                        (1, "Gerenciar listas"),
                        (2, "Gerenciar tarefas"),
                        (3, "Visualizar informações do usuário")
                    })
                    .UseConverter(a => a.Name)
                    .HighlightStyle(Color.Blue)
                    .Title("=== Menu do Usuário ===")
            );
            switch(opcao.Index)
            {
                case 0:
                    return;
                case 1:
                    Console.Clear();
                    MenuListas(listaService, context);
                    break;
                case 2:
                    Console.Clear();
                    MenuTarefas(tarefaService, listaService, context);
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine($"ID: {usuarioAtual.UsuarioId}, Nome: {usuarioAtual.Nome}, Email: {usuarioAtual.Email}");
                    break;
            }
        }
    }

    static void MenuTarefas(TarefaService tarefaService, ListaService listaService, DataContext? context)
    {
        while (true)
        {
           var opcao = AnsiConsole.Prompt(
                new SelectionPrompt<(int Index, string Name)>()
                    .AddChoices( new List<(int Index, string Name)>
                    {
                        (0, "Voltar"),
                        (1, "Adicionar Tarefa"),
                        (2, "Alterar Tarefa"),
                        (3, "Excluir tarefa"),
                        (4, "Buscar tarefas"),
                        (5, "Buscar tarefas por lista"),
                        (6, "Concluir tarefas em lote")
                    })
                    .UseConverter(a => a.Name)
                    .HighlightStyle(Color.Blue)
                    .Title("=== Menu de Tarefas ===")
            );
            switch(opcao.Index)
            {
                case 0:
                    Console.Clear();
                    return;
                case 1:
                    Console.Clear();
                    tarefaService.Add(context, usuarioAtual);
                    break;
                case 2:
                    Console.Clear();
                    tarefaService.UpdateTarefa(context,tarefaService, listaService, usuarioAtual);
                    break;
                case 3:
                    Console.Clear();
                    tarefaService.Delete(tarefaService, listaService, usuarioAtual);
                    break;
                case 4:
                    Console.Clear();
                    tarefaService.GetAllTarefas(tarefaService, listaService, usuarioAtual);
                    break;
                case 5:
                    Console.Clear();
                    tarefaService.GetTarefasPorLista(tarefaService, listaService, usuarioAtual);
                    break;
                case 6:
                    Console.Clear();
                    tarefaService.AlterarStatusEmLote(tarefaService, listaService, usuarioAtual, context);
                    break;
            }
        }
    }

        static void MenuListas(ListaService listaService, DataContext? context)
        {
            while (true)
            {
                var opcao = AnsiConsole.Prompt(
                    new SelectionPrompt<(int Index, string Name)>()
                        .AddChoices( new List<(int Index, string Name)>
                        {
                            (0, "Voltar"),
                            (1, "Adicionar Lista"),
                            (2, "Alterar lista"),
                            (3, "Excluir lista"),
                            (4, "Buscar listas")
                        })
                        .UseConverter(a => a.Name)
                        .HighlightStyle(Color.Blue)
                        .Title("=== Menu de Listas ===")
                );
                switch(opcao.Index)
                {
                    case 0:
                        Console.Clear();
                        return;
                    case 1:
                        Console.Clear();
                        listaService.Add(context, usuarioAtual);
                        break;
                    case 2:
                        Console.Clear();
                        listaService.UpdateLista(context, listaService, usuarioAtual);
                        break;
                    case 3:
                        Console.Clear();
                        listaService.Delete(listaService, usuarioAtual);
                        break;
                    case 4:
                        Console.Clear();
                        listaService.GetAllListas(listaService, usuarioAtual);
                        break;
                }
            }
        }
    }
}
