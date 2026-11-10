using TrilhasApi.Models;

namespace TrilhasApi.DTOs;

public static class TrilhaMapeamento
{
    // Projeção traduzida para SQL: a contagem vira subconsulta, sem carregar os desafios.
    public static IQueryable<TrilhaResponse> ParaResponse(this IQueryable<Trilha> trilhas) =>
        trilhas.Select(t => new TrilhaResponse(
            t.Id,
            t.Titulo,
            t.Descricao,
            t.Tema,
            t.Nivel,
            t.DataCriacao,
            t.Desafios.Count));
}
