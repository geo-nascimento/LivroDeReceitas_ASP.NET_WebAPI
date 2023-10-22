using Bogus;
using MeuLivroDeReceitas.Application.Servicos.Critografia;
using MeuLivroDeReceitas.Comunication.Request;
using MeuLivroDeReceitas.Domain.Entities;
using Utilitario.ParaTestes.Criptografia;

namespace Utilitario.ParaTestes.Entidades;

public class UsuarioBuilder
{
    public static (Usuario usuario, string senha) Contruir()
    {
        string senha = String.Empty;
        
        var usuarioGerado = new Faker<Usuario>()
            .RuleFor(c => c.Id, _ => 1)
            .RuleFor(c => c.Nome, f => f.Person.FullName)
            .RuleFor(c =>c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Senha, f =>
            {
                senha = f.Internet.Password();

                return EncriptadorDeSenhaBuilder.Instancia().Criptografar(senha);
            })
            .RuleFor(c => c.Telefone, f =>
                f.Phone.PhoneNumber("## ! ####-####").Replace("!", $"{f.Random.Int(1, 9)}"));

        return (usuarioGerado, senha);
    }
}