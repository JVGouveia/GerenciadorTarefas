﻿using System.Collections.Generic;
using TestePoo.Data;
using TestePoo.Interfaces;
using TestePoo.Models;

namespace TestePoo.Services
{
    public class ListaService
    {
        private readonly IRepository<Lista> _listaRepository;

        public ListaService(IRepository<Lista> listaRepository)
        {
            _listaRepository = listaRepository;
        }
        
        public void Add(DataContext context, Usuario? usuario)
        {
            string nome;
            
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
                    // return lista;
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
    }
}