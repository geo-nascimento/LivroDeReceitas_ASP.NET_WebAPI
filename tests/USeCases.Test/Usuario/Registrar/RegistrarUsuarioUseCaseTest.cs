using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExecptionsBase;
using Utilitario.ParaTestes.Criptografia;
using Utilitario.ParaTestes.Mapper;
using Utilitario.ParaTestes.Repositorios;
using Utilitario.ParaTestes.Requisicoes;
using Utilitario.ParaTestes.Token;

namespace USeCases.Test.Usuario.Registrar;

public class RegistrarUsuarioUseCaseTest
{
    [Fact]
    public async Task Validar_Requisicao_Sucesso()
    {
        var requisicao = RequisicoesRegistrarUsuarioBuilder.Contruir();
        var useCase = CriarUseCase();

        var resposta = await useCase.Executar(requisicao);

        resposta.Should().NotBeNull();
        resposta.Token.Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task Validar_Requisicao_Erro_Email_Ja_Registrado()
    {
        var requisicao = RequisicoesRegistrarUsuarioBuilder.Contruir();
        var useCase = CriarUseCase(requisicao.Email);
        
        //Como vai lançar uma exception e parar a aplicação eu preciso armazenar o erro para tratar
        //Armazeno em um delegate Func que referencia um função que retorna algum valor
        Func<Task> acao = async () => { await useCase.Executar(requisicao); };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(erro =>
                erro.MensagensDeErro.Count == 1 &&
                erro.MensagensDeErro.Contains(ResourceMensagensDeErro.EMAIL_JA_CADASTRADO));
    }
    
    [Fact]
    public async Task Validar_Requisicao_Erro_Email_Vazio()
    {
        var requisicao = RequisicoesRegistrarUsuarioBuilder.Contruir();
        requisicao.Email = string.Empty;
        
        var useCase = CriarUseCase();
        
        //Como vai lançar uma exception e parar a aplicação eu preciso armazenar o erro para tratar
        //Armazeno em um delegate Func que referencia um função que retorna algum valor
        Func<Task> acao = async () => { await useCase.Executar(requisicao); };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(erro =>
                erro.MensagensDeErro.Count == 1 &&
                erro.MensagensDeErro.Contains(ResourceMensagensDeErro.EMAIL_USUARIO_EMBRANCO));
    }
    
    //Função auxiliar
    private RegistrarUsuarioUseCase CriarUseCase(string email = "")
    {
        //Fazer o mock de todas as deprêndencias de useCase para poder usá-los nos testes
        var repositorioWrite = UsuarioRepositorioWriteOnlyBuilder.Instancia().Construir();
        var repositorioRead = UsuarioReadOnlyRepositorioBuilder.Instancia().ExisteUsuarioComEmail(email).Construir();
        var mapper = MapperBuilder.Instancia();
        var uof = UnitOfWorkBuilder.Instancia().Construir();
        var encriptadorSenha = EncriptadorDeSenhaBuilder.Instancia();
        var token = TokenControllerBuilder.Instancia();
        
        return new RegistrarUsuarioUseCase(repositorioWrite, mapper, uof, encriptadorSenha, token, repositorioRead);
    }
}