using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;
using MeuLivroDeReceitas.Exceptions;
using Utilitario.ParaTestes.Requisicoes;

namespace Validators.Test.Usuario.AlterarSenha;

public class AlterarSenhaValidatorTeste
{
    [Fact]
    public void Validar_Requisicao_Sucesso()
    {
        //Arrange
        var validator = new AlterarSenhaValidator();
        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Contruir();
        
        //Act
        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Validar_Erro_Senha_Fora_Padrao(int tamanhoSenha)
    {
        var validator = new AlterarSenhaValidator();

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Contruir(tamanhoSenha);
        //Act
        var resultado = validator.Validate(requisicao);
        
        //Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceMensagensDeErro.SENHA_USUARIO_MIN_SEIS_CARACTERES));
    }
    
    [Fact]
    public void Validar_Erro_Senha_Vazia()
    {
        var validator = new AlterarSenhaValidator();

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Contruir();
        requisicao.NovaSenha = string.Empty;
        //Act
        var resultado = validator.Validate(requisicao);
        
        //Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceMensagensDeErro.SENHA_USUARIO_EMBRANCO));
    }
}