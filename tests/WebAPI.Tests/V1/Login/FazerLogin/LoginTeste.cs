using System.Net;
using System.Text.Json;
using FluentAssertions;
using MeuLivroDeReceitas.Comunication.Request;
using MeuLivroDeReceitas.Exceptions;
using Utilitario.ParaTestes.Requisicoes;

namespace WebAPI.Tests.V1.Login.FazerLogin;

public class LoginTeste: ControllerBase
{
    private const string METHOD = "login";
    private MeuLivroDeReceitas.Domain.Entities.Usuario _usuario;
    private string _senha;

    public LoginTeste(MeuLivroDeReceitasWebApplicationFactory<Program> factory) : base(factory)
    {
        _usuario = factory.RecuperarUsuario();
        _senha = factory.RecuperarSenha();
    }

    [Fact]
    public async Task Validar_Com_Sucesso()
    {
        var requisisao = new RequisicaoLoginJson()
        {
            Email = _usuario.Email,
            Senha = _senha
        };

        var resposta = await PostRequest(METHOD, requisisao);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        //Testar o token
        await using var responseBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("nome").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_usuario.Nome);
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task Validar_Com_Erro_Email_invalido()
    {
        var requisisao = new RequisicaoLoginJson()
        {
            Email = "emailInvalido",
            Senha = _senha
        };

        var resposta = await PostRequest(METHOD, requisisao);

        resposta.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        //Testar o token
        await using var responseBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var erros = responseData.RootElement.GetProperty("mensagens").Deserialize<List<string>>();
        erros.Should().ContainSingle().And.Contain(ResourceMensagensDeErro.LOGIN_INVALIDO);

    }
    
    [Fact]
    public async Task Validar_Com_Erro_Senha_invalida()
    {
        var requisisao = new RequisicaoLoginJson()
        {
            Email = _usuario.Email,
            Senha = "senhaInvalida"
        };

        var resposta = await PostRequest(METHOD, requisisao);

        resposta.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        //Testar o token
        await using var responseBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var erros = responseData.RootElement.GetProperty("mensagens").Deserialize<List<string>>();
        erros.Should().ContainSingle().And.Contain(ResourceMensagensDeErro.LOGIN_INVALIDO);

    }
    
    [Fact]
    public async Task Validar_Com_Erro_SenhaEmail_invalidos()
    {
        var requisisao = new RequisicaoLoginJson()
        {
            Email = "emailInvalido",
            Senha = "senhaInvalida"
        };

        var resposta = await PostRequest(METHOD, requisisao);

        resposta.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        //Testar o token
        await using var responseBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var erros = responseData.RootElement.GetProperty("mensagens").Deserialize<List<string>>();
        erros.Should().ContainSingle().And.Contain(ResourceMensagensDeErro.LOGIN_INVALIDO);

    }
    
}