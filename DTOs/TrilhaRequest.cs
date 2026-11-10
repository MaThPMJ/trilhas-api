using System.ComponentModel.DataAnnotations;
using TrilhasApi.Models;

namespace TrilhasApi.DTOs;

public record TrilhaRequest(
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 100 caracteres.")]
    string Titulo,

    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    string? Descricao,

    // Anulável para que a ausência do campo seja rejeitada em vez de virar o primeiro valor do enum.
    [Required(ErrorMessage = "O tema é obrigatório.")]
    Tema? Tema,

    [Range(1, 3, ErrorMessage = "O nível deve estar entre 1 e 3.")]
    int Nivel);
