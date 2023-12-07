using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestePoo.Models;

[Table("tb_usuario")]
public class Usuario
{
    public Usuario(string nome, string email, string senha)
    {
        Nome = nome;
        Email = email;
        Senha = senha;
    }

    [Column("id_usuario")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UsuarioId { get; set; }
    [Column("nome")]
    public string Nome { get; set; }
    [Column("email")]
    public string Email { get; set; }
    [Column("senha")]
    [DataType(DataType.Password)]
    public string Senha { get; set; }

    public List<Lista> Listas { get; set; }
}