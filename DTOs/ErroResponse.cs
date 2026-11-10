using System.Text.Json.Serialization;

namespace TrilhasApi.DTOs;

public record ErroResponse(
    string Erro,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    IDictionary<string, string[]>? Detalhes = null);
