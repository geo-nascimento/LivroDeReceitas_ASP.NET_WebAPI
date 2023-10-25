using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Domain.Repositories;
using Moq;

namespace Utilitario.ParaTestes.Repositorios;

public class UsuarioUpdateOnlyRepositorioBuilder
{
    private static UsuarioUpdateOnlyRepositorioBuilder _instance;
    private readonly Mock<IUsuarioUpdateOnly> _repositorio;

    private UsuarioUpdateOnlyRepositorioBuilder()
    {
        if (_repositorio == null)
        {
            _repositorio = new Mock<IUsuarioUpdateOnly>();
        }
    }

    public static UsuarioUpdateOnlyRepositorioBuilder Instancia()
    {
        _instance = new UsuarioUpdateOnlyRepositorioBuilder();
        return _instance;
    }

    public UsuarioUpdateOnlyRepositorioBuilder RecuperarPorId(Usuario usuario)
    {
        _repositorio.Setup(c => c.RecuperarPorId(usuario.Id)).ReturnsAsync(usuario);
        return this;
    }

    public IUsuarioUpdateOnly Construir()
    {
        return _repositorio.Object;
    }
}