using TestePoo.Services;

namespace TestePoo.Controllers;

public class TarefaController
{
    private readonly TarefaService _tarefaService;

    public TarefaController(TarefaService tarefaService)
    {
        _tarefaService = tarefaService;
    }
}