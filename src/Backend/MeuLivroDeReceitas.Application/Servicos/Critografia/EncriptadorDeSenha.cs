using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MeuLivroDeReceitas.Application.Servicos.Critografia;

public class EncriptadorDeSenha
{
    private readonly string _chaveDeEncriptacao;

    public EncriptadorDeSenha(string chaveDeEncriptacao)
    {
        _chaveDeEncriptacao = chaveDeEncriptacao;
    }
    
    public string Criptografar(string senha) //Baseada em hash que só criptografa, ele não traduz
    {
        var senhaComChaveAdcional = $"{senha}{_chaveDeEncriptacao}";
        
        var bytes = Encoding.UTF8.GetBytes(senhaComChaveAdcional);
        var sha512 = SHA512.Create();
        byte[] hasBytes = sha512.ComputeHash(bytes);
        return StringBytes(hasBytes);
    }

    private static string StringBytes(byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (var b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }

        return sb.ToString();
    }
}