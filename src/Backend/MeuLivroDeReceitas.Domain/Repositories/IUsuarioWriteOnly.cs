using MeuLivroDeReceitas.Domain.Entities;

namespace MeuLivroDeReceitas.Domain.Repositories;

public interface IUsuarioWriteOnly
{
    Task Adicionar(Usuario usuario);
}