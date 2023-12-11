using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Migrations;
using TestePoo.Data;
using TestePoo.Repositories;
using TestePoo.Services;

namespace TestePoo.Models;

[Table("tb_lista")]
public class Lista
{
    [Column("id_lista")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ListaId { get; set; }
    [Column("nome")]
    public string? Nome { get; set; }
    [Column("id_usuario")]
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public List<Tarefa> Tarefas { get; set; }
    
    public Lista(string? nome, int usuarioId)
    {
        Nome = nome;
        UsuarioId = usuarioId;
        // Usuario = usuario;
    }

    public override string ToString()
    {
        return $"{ListaId}. {Nome}";
    }
}