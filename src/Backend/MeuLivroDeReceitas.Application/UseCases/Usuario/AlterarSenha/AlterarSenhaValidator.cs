using FluentValidation;
using MeuLivroDeReceitas.Comunication.Request;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;

public class AlterarSenhaValidator : AbstractValidator<RequestAlterarSenhaJson>
{
    public AlterarSenhaValidator()
    {
        RuleFor(c => c.NovaSenha).SetValidator(new SenhaValidator());
    }
}