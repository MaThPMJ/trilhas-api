using System.ComponentModel.DataAnnotations;

namespace TrilhasApi.DTOs;

public record DesafioRequest(
    [Range(1, int.MaxValue, ErrorMessage = "A trilha do desafio é obrigatória.")]
    int TrilhaId,

    [Required(ErrorMessage = "A pergunta é obrigatória.")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "A pergunta deve ter entre 10 e 500 caracteres.")]
    string Pergunta,

    [Required(ErrorMessage = "A alternativa A é obrigatória.")]
    [StringLength(200, ErrorMessage = "A alternativa A deve ter no máximo 200 caracteres.")]
    string AlternativaA,

    [Required(ErrorMessage = "A alternativa B é obrigatória.")]
    [StringLength(200, ErrorMessage = "A alternativa B deve ter no máximo 200 caracteres.")]
    string AlternativaB,

    [Required(ErrorMessage = "A alternativa C é obrigatória.")]
    [StringLength(200, ErrorMessage = "A alternativa C deve ter no máximo 200 caracteres.")]
    string AlternativaC,

    [Required(ErrorMessage = "A alternativa D é obrigatória.")]
    [StringLength(200, ErrorMessage = "A alternativa D deve ter no máximo 200 caracteres.")]
    string AlternativaD,

    [Required(ErrorMessage = "A resposta correta é obrigatória.")]
    [RegularExpression("^[A-Da-d]$", ErrorMessage = "A resposta correta deve ser A, B, C ou D.")]
    string RespostaCorreta,

    [Range(1, 100, ErrorMessage = "Os pontos devem estar entre 1 e 100.")]
    int Pontos);
