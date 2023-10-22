using MeuLivroDeReceitas.Domain.Repositories;
using Moq;

namespace Utilitario.ParaTestes.Repositorios;

public class UsuarioRepositorioWriteOnlyBuilder
{
    private static UsuarioRepositorioWriteOnlyBuilder _instance;
    private readonly Mock<IUsuarioWriteOnly> _repositorio;

    private UsuarioRepositorioWriteOnlyBuilder()
    {
        if (_repositorio == null)
        {
            _repositorio = new Mock<IUsuarioWriteOnly>();
        }
    }

    public static UsuarioRepositorioWriteOnlyBuilder Instancia()
    {
        _instance = new UsuarioRepositorioWriteOnlyBuilder();
        return _instance;
    }

    public IUsuarioWriteOnly Construir()
    {
        return _repositorio.Object;
    }
}