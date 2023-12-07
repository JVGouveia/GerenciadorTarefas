using TestePoo.Services;

namespace TestePoo.Controllers;

public class ListaController
{
    private readonly ListaService _listaService;

    public ListaController(ListaService listaService)
    {
        _listaService = listaService;
    }
}