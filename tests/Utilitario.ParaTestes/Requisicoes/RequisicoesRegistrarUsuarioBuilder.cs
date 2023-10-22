using Bogus;
using MeuLivroDeReceitas.Comunication.Request;

namespace Utilitario.ParaTestes.Requisicoes;

public  static class RequisicoesRegistrarUsuarioBuilder
{
    public static RequestRegistrasUsuarioJson Contruir(int tamanhoSenha = 10)
    {
        return new Faker<RequestRegistrasUsuarioJson>().
                RuleFor(c => c.Nome, f => f.Person.FullName)
                .RuleFor(c =>c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Senha, f =>f.Internet.Password(tamanhoSenha))
                .RuleFor(c => c.Telefone, f =>
                    f.Phone.PhoneNumber("## ! ####-####").Replace("!", $"{f.Random.Int(1, 9)}"));
    }
}