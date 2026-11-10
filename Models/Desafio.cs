using TrilhasApi.Exceptions;

namespace TrilhasApi.Models;

public class Desafio
{
    private static readonly string[] RespostasValidas = { "A", "B", "C", "D" };

    public int Id { get; private set; }
    public int TrilhaId { get; private set; }
    public Trilha Trilha { get; private set; } = null!;
    public string Pergunta { get; private set; } = string.Empty;
    public string AlternativaA { get; private set; } = string.Empty;
    public string AlternativaB { get; private set; } = string.Empty;
    public string AlternativaC { get; private set; } = string.Empty;
    public string AlternativaD { get; private set; } = string.Empty;
    public string RespostaCorreta { get; private set; } = string.Empty;
    public int Pontos { get; private set; }

    // O EF Core precisa de um construtor sem parâmetros para materializar a entidade.
    protected Desafio()
    {
    }

    public Desafio(
        int trilhaId,
        string pergunta,
        string alternativaA,
        string alternativaB,
        string alternativaC,
        string alternativaD,
        string respostaCorreta,
        int pontos)
    {
        Definir(trilhaId, pergunta, alternativaA, alternativaB, alternativaC, alternativaD, respostaCorreta, pontos);
    }

    public void Atualizar(
        int trilhaId,
        string pergunta,
        string alternativaA,
        string alternativaB,
        string alternativaC,
        string alternativaD,
        string respostaCorreta,
        int pontos)
    {
        Definir(trilhaId, pergunta, alternativaA, alternativaB, alternativaC, alternativaD, respostaCorreta, pontos);
    }

    private void Definir(
        int trilhaId,
        string pergunta,
        string alternativaA,
        string alternativaB,
        string alternativaC,
        string alternativaD,
        string respostaCorreta,
        int pontos)
    {
        if (trilhaId <= 0)
            throw new DomainException("A trilha do desafio é obrigatória.");

        if (string.IsNullOrWhiteSpace(pergunta))
            throw new DomainException("A pergunta é obrigatória.");

        pergunta = pergunta.Trim();
        if (pergunta.Length < 10 || pergunta.Length > 500)
            throw new DomainException("A pergunta deve ter entre 10 e 500 caracteres.");

        alternativaA = ValidarAlternativa(alternativaA, "A");
        alternativaB = ValidarAlternativa(alternativaB, "B");
        alternativaC = ValidarAlternativa(alternativaC, "C");
        alternativaD = ValidarAlternativa(alternativaD, "D");

        var alternativas = new[] { alternativaA, alternativaB, alternativaC, alternativaD };
        if (alternativas.Distinct(StringComparer.OrdinalIgnoreCase).Count() != alternativas.Length)
            throw new DomainException("As alternativas devem ser distintas entre si.");

        respostaCorreta = (respostaCorreta ?? string.Empty).Trim().ToUpperInvariant();
        if (!RespostasValidas.Contains(respostaCorreta))
            throw new DomainException("A resposta correta deve ser A, B, C ou D.");

        if (pontos < 1 || pontos > 100)
            throw new DomainException("Os pontos devem estar entre 1 e 100.");

        TrilhaId = trilhaId;
        Pergunta = pergunta;
        AlternativaA = alternativaA;
        AlternativaB = alternativaB;
        AlternativaC = alternativaC;
        AlternativaD = alternativaD;
        RespostaCorreta = respostaCorreta;
        Pontos = pontos;
    }

    private static string ValidarAlternativa(string valor, string letra)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException($"A alternativa {letra} é obrigatória.");

        valor = valor.Trim();
        if (valor.Length > 200)
            throw new DomainException($"A alternativa {letra} deve ter no máximo 200 caracteres.");

        return valor;
    }
}
