using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.DateTime;

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

    [Column("data_limite")] 
    public DateTime DataLimite { get; set; }
    [Column("status"), DefaultValue(0)]
    public int Status { get; set; } // 0 = Pendente e 1 = Concluida
    
    [Column("id_lista")]
    public int ListaId { get; set; }
    public Lista Lista { get; set; }
    
    public Tarefa(int tarefaId, string nome, string descricao, DateTime dataLimite, int status, int listaId)
    {
        TarefaId = tarefaId;
        Nome = nome;
        Descricao = descricao;
        DataLimite = dataLimite;
        Status = status;
        ListaId = listaId;
    }
    public Tarefa(string nome, string descricao, DateTime dataLimite, int listaId)
    {
        Nome = nome;
        Descricao = descricao;
        DataLimite = dataLimite;
        Status = 0;
        ListaId = listaId;
    }
}
