using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Login.FazerLogin;
using MeuLivroDeReceitas.Comunication.Request;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExecptionsBase;
using Utilitario.ParaTestes.Criptografia;
using Utilitario.ParaTestes.Entidades;
using Utilitario.ParaTestes.Repositorios;
using Utilitario.ParaTestes.Token;

namespace USeCases.Test.Login.FazerLogin;

public class LoginUseCaseTeste
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        (var usuario, var senha) = UsuarioBuilder.Contruir();

        var useCase = CriarUseCase(usuario);
        var resposta = await useCase.Executar(new RequisicaoLoginJson()
        {
            Email = usuario.Email,
            Senha = senha
        });

        resposta.Should().NotBeNull();
        resposta.Nome.Should().Be(usuario.Nome);
        resposta.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Validar_Login_Erro_LoginInvalido_Senha_invalida()
    {
        (var usuario, var senha) = UsuarioBuilder.Contruir();

        var useCase = CriarUseCase(usuario);

        Func<Task> acao = async () =>
        {
            await useCase.Executar(new RequisicaoLoginJson()
            {
                Email = usuario.Email,
                Senha = "SenhainValida"
            });
        };

        await acao.Should().ThrowAsync<LoginInvalidoException>()
            .Where(exp => exp.Message.Equals(ResourceMensagensDeErro.LOGIN_INVALIDO));
    }
    
    [Fact]
    public async Task Validar_Login_Erro_LoginInvalido_Email_invalido()
    {
        (var usuario, var senha) = UsuarioBuilder.Contruir();

        var useCase = CriarUseCase(usuario);

        Func<Task> acao = async () =>
        {
            await useCase.Executar(new RequisicaoLoginJson()
            {
                Email = "emailInvalido",
                Senha = senha
            });
        };

        await acao.Should().ThrowAsync<LoginInvalidoException>()
            .Where(exp => exp.Message.Equals(ResourceMensagensDeErro.LOGIN_INVALIDO));
    }
    
    [Fact]
    public async Task Validar_Login_Erro_LoginInvalido_SenhaEmail_invalida()
    {
        (var usuario, var senha) = UsuarioBuilder.Contruir();

        var useCase = CriarUseCase(usuario);

        Func<Task> acao = async () =>
        {
            await useCase.Executar(new RequisicaoLoginJson()
            {
                Email = "emailInvalido",
                Senha = "SenhainValida"
            });
        };

        await acao.Should().ThrowAsync<LoginInvalidoException>()
            .Where(exp => exp.Message.Equals(ResourceMensagensDeErro.LOGIN_INVALIDO));
    }
    
    //Criar useCase
    private LoginUseCase CriarUseCase(MeuLivroDeReceitas.Domain.Entities.Usuario usuario)
    {
        var repositorioRead = UsuarioReadOnlyRepositorioBuilder.Instancia().RecuperarPorEmailSenha(usuario).Construir();
        var encriptadorSenha = EncriptadorDeSenhaBuilder.Instancia();
        var token = TokenControllerBuilder.Instancia();
        
        return new LoginUseCase(repositorioRead,encriptadorSenha, token);
    }
}