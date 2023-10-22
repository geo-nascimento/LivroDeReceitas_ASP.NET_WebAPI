using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Infrastructure.AcessoRepository;
using Utilitario.ParaTestes.Entidades;

namespace WebAPI.Tests;

public class ContextSeedInMemory
{
    public static (Usuario usuario, string senha)  Seed(LivroDeReceitasContext context)
    {
        (var usuario, var senha) = UsuarioBuilder.Contruir();
        
        context.Usuarios.Add(usuario);

        context.SaveChanges();

        return (usuario, senha);
    }
}