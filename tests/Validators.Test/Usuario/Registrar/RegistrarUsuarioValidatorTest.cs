using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;
using MeuLivroDeReceitas.Comunication.Request;
using MeuLivroDeReceitas.Exceptions;
using Utilitario.ParaTestes.Requisicoes;

namespace Validators.Test.Usuario.Registrar;

public class RegistrarUsuarioValidatorTest
{
    [Fact]
    public void Validar_Erro_Nome_Vazio() 
    {
        //Arrange
        var validator = new RegistrarUsuarioValidator();

        var requisicao = RequisicoesRegistrarUsuarioBuilder.Contruir();
        requisicao.Nome = string.Empty;
        //Act
        var resultado = validator.Validate(requisicao);
        
        //Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceMensagensDeErro.NOME_USUARIO_EMBRANCO));
    }

    [Fact]
    public void Validar_Requisicao_Sucesso()
    {
        //Arrange
        var validator = new RegistrarUsuarioValidator();
        var requisicao = RequisicoesRegistrarUsuarioBuilder.Contruir();
        
        //Act
        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validar_Erro_Email_Vazio()
    {
        var validator = new RegistrarUsuarioValidator();

        var requisicao = RequisicoesRegistrarUsuarioBuilder.Contruir();
        requisicao.Email = string.Empty;
        //Act
        var resultado = validator.Validate(requisicao);
        
        //Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceMensagensDeErro.EMAIL_USUARIO_EMBRANCO));
    }
    
    [Fact]
    public void Validar_Erro_Senha_Vazia()
    {
        var validator = new RegistrarUsuarioValidator();

        var requisicao = RequisicoesRegistrarUsuarioBuilder.Contruir();
        requisicao.Senha = string.Empty;
        //Act
        var resultado = validator.Validate(requisicao);
        
        //Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceMensagensDeErro.SENHA_USUARIO_EMBRANCO));
    }
    
    [Fact]
    public void Validar_Erro_Telefone_Vazio()
    {
        var validator = new RegistrarUsuarioValidator();

        var requisicao = RequisicoesRegistrarUsuarioBuilder.Contruir();
        requisicao.Telefone = string.Empty;
        //Act
        var resultado = validator.Validate(requisicao);
        
        //Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceMensagensDeErro.TELEFONE_USUARIO_EMBRANCO));
    }
    
    [Fact]
    public void Validar_Erro_Telefone_Fora_Padrao()
    {
        var validator = new RegistrarUsuarioValidator();

        var requisicao = RequisicoesRegistrarUsuarioBuilder.Contruir();
        requisicao.Telefone = "993390333";
        //Act
        var resultado = validator.Validate(requisicao);
        
        //Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceMensagensDeErro.TELEFONE_USUARIO_INVALIDO));
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Validar_Erro_Senha_Fora_Padrao(int tamanhoSenha)
    {
        var validator = new RegistrarUsuarioValidator();

        var requisicao = RequisicoesRegistrarUsuarioBuilder.Contruir(tamanhoSenha);
        //Act
        var resultado = validator.Validate(requisicao);
        
        //Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceMensagensDeErro.SENHA_USUARIO_MIN_SEIS_CARACTERES));
    }
    
    [Fact]
    public void Validar_Erro_Email_Fora_Padrao()
    {
        var validator = new RegistrarUsuarioValidator();

        var requisicao = RequisicoesRegistrarUsuarioBuilder.Contruir();
        requisicao.Email = "teste";
        //Act
        var resultado = validator.Validate(requisicao);
        
        //Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(error =>
            error.ErrorMessage.Equals(ResourceMensagensDeErro.EMAIL_USUARIO_INVALIDO));
    }
}