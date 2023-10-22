using MeuLivroDeReceitas.Domain.Repositories;
using MeuLivroDeReceitas.Infrastructure.AcessoRepository.Repository;
using Moq;

namespace Utilitario.ParaTestes.Repositorios;

public class UnitOfWorkBuilder
{
    private static UnitOfWorkBuilder _instance;
    private readonly Mock<IUnitOfWork> _uof;

    private UnitOfWorkBuilder()
    {
        if (_uof == null)
        {
            _uof = new Mock<IUnitOfWork>();
        }
    }

    public static UnitOfWorkBuilder Instancia()
    {
        _instance = new UnitOfWorkBuilder();
        return _instance;
    }

    public IUnitOfWork Construir()
    {
        return _uof.Object;
    }
}