using TestePoo.Services;

namespace TestePoo.Controllers.Usuario;

public class UsuarioController
{
    private readonly UsuarioService _usuarioService;

    public UsuarioController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }
}