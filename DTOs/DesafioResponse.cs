namespace TrilhasApi.DTOs;

public record DesafioResponse(
    int Id,
    int TrilhaId,
    string TituloTrilha,
    string Pergunta,
    string AlternativaA,
    string AlternativaB,
    string AlternativaC,
    string AlternativaD,
    string RespostaCorreta,
    int Pontos);
