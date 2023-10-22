namespace MeuLivroDeReceitas.Infrastructure.AcessoRepository.Repository;

public interface IUnitOfWork
{
    Task Commit();
}