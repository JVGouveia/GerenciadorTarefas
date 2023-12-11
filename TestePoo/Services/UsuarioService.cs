using System.Globalization;
using System.Text.RegularExpressions;
using TestePoo.Data;
using TestePoo.Models;
using TestePoo.Repositories;

namespace TestePoo.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }


        static bool ValidaEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }

        public Usuario Add(DataContext? context)
        {
            string nome;
            string email;
            string senha;
            
            while (true)
            {
                Console.WriteLine("Informe seu nome:");
                nome = Console.ReadLine();

                while (string.IsNullOrEmpty(nome.Trim()))
                {
                    Console.WriteLine("Nome não pode estar vazio! Digite novamente:");
                    nome = Console.ReadLine();
                }

                nome = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nome.Trim());
                break;
            }

            while (true)
            {
                Console.WriteLine("Informe seu Email:");
                email = Console.ReadLine();
                while (string.IsNullOrEmpty(email.Trim()) || !ValidaEmail(email))
                {
                    Console.WriteLine("Email não pode estar vazio ou não é um formato válido! Digite novamente:");
                    email = Console.ReadLine();
                }
                if (_usuarioRepository.ExisteNaBaseDeDados("tb_usuario", "email", email) != null)
                    Console.WriteLine("Este e-mail já está cadastrado.");
                else
                    break;            
            }

            while (true)
            {
                Console.WriteLine("Informe sua senha:");
                senha = Console.ReadLine();

                while (string.IsNullOrEmpty(senha.Trim()))
                {
                    Console.WriteLine("Senha inválida! Digite novamente:");
                    senha = Console.ReadLine();
                }
                break;
            }
                
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Usuario? usuario = new Usuario(nome, email, senha);
                    _usuarioRepository.Add(usuario);
                    transaction.Commit();
                    return usuario;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao criar o usuário: {e.Message}");
                    transaction.Rollback();
                    throw;
                }
            }
        }
        
        public Usuario? ValidarUsuario(DataContext? context)
        {
            string email, senha;
            Usuario? databaseUsuario;

            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("Informe seu Email:");
                email = Console.ReadLine();
                
                while (string.IsNullOrEmpty(email) || !ValidaEmail(email))
                {
                    Console.WriteLine("Email não pode estar vazio ou não é um formato válido! Digite novamente:");
                    email = Console.ReadLine();
                }

                databaseUsuario = _usuarioRepository.ExisteNaBaseDeDados("tb_usuario", "Email", email);
                if (databaseUsuario == null)
                    Console.WriteLine("Email não cadastrado no banco de dados! Digite novamente ou digite [sair]");
                else
                    break;
            }

            while (true)
            {
                Console.WriteLine("Digite sua senha");
                senha = Console.ReadLine();
                if (senha != databaseUsuario.Senha)
                    Console.WriteLine("Senha incorreta!");
                else
                    break;
            }

            return databaseUsuario;
        }

        public Usuario GetById(int id)
        {
            return _usuarioRepository.GetById(id);
        }

        public IEnumerable<Usuario> GetAll()
        {
            return _usuarioRepository.GetAll();
        }

        public void Update(Usuario usuario)
        {
            _usuarioRepository.Update(usuario);
        }

        public void Delete(int id)
        {
            _usuarioRepository.Delete(id);
        }
    }
}