using TrilhasApi.Models;

namespace TrilhasApi.DTOs;

public record TrilhaResponse(
    int Id,
    string Titulo,
    string? Descricao,
    Tema Tema,
    int Nivel,
    DateTime DtCriacao,
    int QuantidadeDesafios);
