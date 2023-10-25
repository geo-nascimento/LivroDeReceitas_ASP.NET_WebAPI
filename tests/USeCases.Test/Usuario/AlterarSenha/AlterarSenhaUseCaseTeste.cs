using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;
using MeuLivroDeReceitas.Comunication.Request;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExecptionsBase;
using Utilitario.ParaTestes.Criptografia;
using Utilitario.ParaTestes.Entidades;
using Utilitario.ParaTestes.Repositorios;
using Utilitario.ParaTestes.Requisicoes;
using Utilitario.ParaTestes.UsuarioLogado;

namespace USeCases.Test.Usuario.AlterarSenha;

public class AlterarSenhaUseCaseTeste
{
    [Fact]
    public async Task Testar_Alter_Senha_Sucesso()
    {
        (var usuario, var senha) = UsuarioBuilder.Contruir();

        var useCase = CriarUseCase(usuario);

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Contruir();
        requisicao.SenhaAtual = senha;

        Func<Task> acao = async () =>
        {
            await useCase.Executar(requisicao);
        };

       await acao.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task Testar_Validar_Senha_EmBramco_Erro()
    {
        (var usuario, var senha) = UsuarioBuilder.Contruir();

        var useCase = CriarUseCase(usuario);
        

        Func<Task> acao = async () =>
        {
            await useCase.Executar(new RequestAlterarSenhaJson()
            {
                SenhaAtual = senha,
                NovaSenha = ""
            });
        };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>().Where(ex =>
            ex.MensagensDeErro.Count == 1 &&
            ex.MensagensDeErro.Contains(ResourceMensagensDeErro.SENHA_USUARIO_EMBRANCO));
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Testar_Validar_SenhaAtual_Diferente_Erro(int tamanhoSenha)
    {
        (var usuario, var senha) = UsuarioBuilder.Contruir();

        var useCase = CriarUseCase(usuario);
        
        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Contruir(tamanhoSenha);
        requisicao.SenhaAtual = senha;

        Func<Task> acao = async () =>
        {
            await useCase.Executar(requisicao);
        };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>().Where(ex =>
            ex.MensagensDeErro.Count == 1 &&
            ex.MensagensDeErro.Contains(ResourceMensagensDeErro.SENHA_USUARIO_MIN_SEIS_CARACTERES));
    }
    
    private AlterarSenhaUSeCase CriarUseCase(MeuLivroDeReceitas.Domain.Entities.Usuario usuario)
    {
        var encriptador = EncriptadorDeSenhaBuilder.Instancia();
        var uof = UnitOfWorkBuilder.Instancia().Construir();
        var repoUpdate = UsuarioUpdateOnlyRepositorioBuilder.Instancia().RecuperarPorId(usuario).Construir();
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
        
        return new AlterarSenhaUSeCase(repoUpdate,usuarioLogado, encriptador, uof);
    }
}