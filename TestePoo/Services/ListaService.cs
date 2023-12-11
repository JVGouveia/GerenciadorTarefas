using System.Collections.Generic;
using ConsoleTables;
using TestePoo.Data;
using TestePoo.Interfaces;
using TestePoo.Models;
using TestePoo.Repositories;

namespace TestePoo.Services
{
    public class ListaService
    {
        private readonly ListaRepository _listaRepository;

        public ListaService(ListaRepository listaRepository)
        {
            _listaRepository = listaRepository;
        }
        
        public void Add(DataContext context, Usuario? usuario)
        {
            string? nome;
            
            while (true)
            {
                Console.WriteLine("Informe o nome da lista");
                nome = Console.ReadLine();
                if (string.IsNullOrEmpty(nome.Trim()))
                    Console.WriteLine("Nome não pode estar vázio! Digite novamente:");
                else
                    break;
            }

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Lista lista = new Lista(nome, usuario.UsuarioId);
                    _listaRepository.Add(lista);
                    transaction.Commit();
                    Console.Clear();
                    Console.WriteLine("Lista adicionada com sucesso");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao criar a lista: {e.Message}");
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public Lista GetById(int id)
        {
            return _listaRepository.GetById(id);
        }

        public IEnumerable<Lista> GetAll()
        {
            return _listaRepository.GetAll();
        }

        public void Update(Lista lista)
        {
            _listaRepository.Update(lista);
        }
        
        public void Delete(int id)
        {
            _listaRepository.Delete(id);
        }

        public void Delete(ListaService listaService, Usuario usuario)
        {
            int ListaId = EscolherLista(listaService, usuario);

            
            Lista listaParaExcluir = GetById(ListaId);
            if (listaParaExcluir != null && GetById(listaParaExcluir.ListaId).UsuarioId == usuario.UsuarioId)
            {
                Delete(ListaId);
                Console.Clear();
                Console.WriteLine("Tarefa excluída com sucesso");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Você não tem permissão para excluir esta tarefa ou a tarefa não existe.");
            }
        }
        
        public List<Lista> GetListasPorUsuario(int usuarioId)
        {
            return _listaRepository.GetListasPorUsuario(usuarioId);
        }

        public int EscolherLista(ListaService listaService, Usuario usuario)
        {
            return _listaRepository.EscolherLista(listaService, usuario);
        }
        
        public void UpdateLista(DataContext? context, ListaService listaService, Usuario usuario)
        {
            int listaId = listaService.EscolherLista(listaService, usuario);
            string nome;

            int usuarioDaLista = GetById(listaId).UsuarioId;

            if (usuarioDaLista != usuario.UsuarioId)
            {
                Console.WriteLine("Você não tem permissão para atualizar esta lista.");
                return;
            }
            
            do
            {
                Console.Write("Informe o novo nome para a lista:");
                nome = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(nome));
            
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Lista listaAtualizada = listaService.GetById(listaId);
                    
                    listaAtualizada.Nome = nome;
                    
                    listaService.Update(listaAtualizada);
                    transaction.Commit();
                    Console.Clear();
                    Console.WriteLine("Lista alterada com sucesso");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao atualizar a lista: {e.Message}");
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public void GetAllListas(ListaService listaService, Usuario usuario)
        {
            var listas = listaService.GetListasPorUsuario(usuario.UsuarioId);
            if (listas.Any())
            {
                Console.WriteLine("\nListas:");
                var listasT = new ConsoleTable("Id", "Nome");

                foreach (var lista in listas)
                {
                    listasT.AddRow($"{lista.ListaId.ToString()}", $"{lista.Nome}");
                }

                listasT.Configure(o => o.EnableCount = false)
                    .Write(Format.Minimal);
            }
            else 
            {
                Console.WriteLine("Não há listas nesse usuario.");
            }
        }
    }
}