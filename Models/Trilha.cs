using TrilhasApi.Exceptions;

namespace TrilhasApi.Models;

public class Trilha
{
    private readonly List<Desafio> _desafios = new();

    public int Id { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public Tema Tema { get; private set; }
    public int Nivel { get; private set; }
    public DateTime DataCriacao { get; private set; }

    public IReadOnlyCollection<Desafio> Desafios => _desafios.AsReadOnly();

    // O EF Core precisa de um construtor sem parâmetros para materializar a entidade.
    protected Trilha()
    {
    }

    public Trilha(string titulo, string? descricao, Tema tema, int nivel)
    {
        Definir(titulo, descricao, tema, nivel);
        DataCriacao = DateTime.Now;
    }

    public void Atualizar(string titulo, string? descricao, Tema tema, int nivel)
    {
        Definir(titulo, descricao, tema, nivel);
    }

    private void Definir(string titulo, string? descricao, Tema tema, int nivel)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("O título é obrigatório.");

        titulo = titulo.Trim();
        if (titulo.Length < 3 || titulo.Length > 100)
            throw new DomainException("O título deve ter entre 3 e 100 caracteres.");

        descricao = string.IsNullOrWhiteSpace(descricao) ? null : descricao.Trim();
        if (descricao is { Length: > 500 })
            throw new DomainException("A descrição deve ter no máximo 500 caracteres.");

        if (!Enum.IsDefined(tema))
            throw new DomainException("O tema informado é inválido.");

        if (nivel < 1 || nivel > 3)
            throw new DomainException("O nível deve estar entre 1 e 3.");

        Titulo = titulo;
        Descricao = descricao;
        Tema = tema;
        Nivel = nivel;
    }
}
