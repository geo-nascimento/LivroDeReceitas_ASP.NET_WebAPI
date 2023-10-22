using FluentAssertions;
using System.Net;
using System.Text.Json;
using MeuLivroDeReceitas.Exceptions;
using Utilitario.ParaTestes.Requisicoes;

namespace WebAPI.Tests.V1.Usuario.Registrar;

public class RegistrarUsuarioTeste : ControllerBase
{
    private const string METHOD = "usuarios";

    public RegistrarUsuarioTeste(MeuLivroDeReceitasWebApplicationFactory<Program> factory) : base(factory)
    {
        
    }

    [Fact]
    public async Task Validar_Com_Sucesso()
    {
        var requisisao = RequisicoesRegistrarUsuarioBuilder.Contruir();

        var resposta = await PostRequest(METHOD, requisisao);

        resposta.StatusCode.Should().Be(HttpStatusCode.Created);

        //Testar o token
        await using var responseBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task Validar_Com_Erro_Nome_Vazio()
    {
        var requisisao = RequisicoesRegistrarUsuarioBuilder.Contruir();
        requisisao.Nome = string.Empty;
        var resposta = await PostRequest(METHOD, requisisao);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        //Testar o token
        await using var responseBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var erros = responseData.RootElement.GetProperty("mensagens").Deserialize<List<string>>();
        erros.Should().ContainSingle().And.Contain(ResourceMensagensDeErro.NOME_USUARIO_EMBRANCO);
    }
}
