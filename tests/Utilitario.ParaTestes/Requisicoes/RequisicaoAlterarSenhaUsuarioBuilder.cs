using Bogus;
using MeuLivroDeReceitas.Comunication.Request;

namespace Utilitario.ParaTestes.Requisicoes;

public static class RequisicaoAlterarSenhaUsuarioBuilder
{
    public static RequestAlterarSenhaJson Contruir(int tamanhoSenha = 10)
    {
        return new Faker<RequestAlterarSenhaJson>()
            .RuleFor(c => c.SenhaAtual, f => f.Internet.Password(10))
            .RuleFor(c => c.NovaSenha, f => f.Internet.Password(tamanhoSenha));
    }
}