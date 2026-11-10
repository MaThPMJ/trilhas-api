using TrilhasApi.Models;

namespace TrilhasApi.DTOs;

public static class DesafioMapeamento
{
    public static IQueryable<DesafioResponse> ParaResponse(this IQueryable<Desafio> desafios) =>
        desafios.Select(d => new DesafioResponse(
            d.Id,
            d.TrilhaId,
            d.Trilha.Titulo,
            d.Pergunta,
            d.AlternativaA,
            d.AlternativaB,
            d.AlternativaC,
            d.AlternativaD,
            d.RespostaCorreta,
            d.Pontos));
}
