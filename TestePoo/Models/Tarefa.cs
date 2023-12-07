using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestePoo.Models;

[Table("tb_tarefa")]
public class Tarefa
{
    [Column("id_tarefa")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TarefaId { get; set; }
    [Column("nome")]
    public string Nome { get; set; }
    [Column("descricao")]
    public string Descricao { get; set; }

    [Column("data_final")]
    public DateTime DataLimite { get; set; }
    [Column("status", TypeName = "enum('Pendente','EmAndamento','Atrasada','Concluida')")]
    [EnumDataType(typeof(StatusTarefa))]
    public StatusTarefa Status { get; set; }
    
    [Column("id_lista")]
    public int ListaId { get; set; }
    public Lista Lista { get; set; }

    public Tarefa(string nome, string descricao, DateTime dataLimite, int listaId)
    {
        Nome = nome;
        Descricao = descricao;
        DataLimite = dataLimite;
        Status = StatusTarefa.Pendente;
        ListaId = listaId;
    }
}
