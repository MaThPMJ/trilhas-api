using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhasApi.Data;
using TrilhasApi.DTOs;
using TrilhasApi.Models;

namespace TrilhasApi.Controllers;

[ApiController]
[Route("api/desafios")]
[Produces("application/json")]
public class DesafiosController : ControllerBase
{
    private readonly AppDbContext _context;

    public DesafiosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DesafioResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DesafioResponse>>> Listar([FromQuery] int? trilhaId)
    {
        var consulta = _context.Desafios.AsNoTracking();

        if (trilhaId.HasValue)
            consulta = consulta.Where(d => d.TrilhaId == trilhaId.Value);

        var desafios = await consulta
            .OrderBy(d => d.Id)
            .ParaResponse()
            .ToListAsync();

        return Ok(desafios);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DesafioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DesafioResponse>> ObterPorId(int id)
    {
        var desafio = await BuscarResponseAsync(id);

        return desafio is null ? NaoEncontrado(id) : Ok(desafio);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DesafioResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DesafioResponse>> Criar(DesafioRequest request)
    {
        var desafio = new Desafio(
            request.TrilhaId,
            request.Pergunta,
            request.AlternativaA,
            request.AlternativaB,
            request.AlternativaC,
            request.AlternativaD,
            request.RespostaCorreta,
            request.Pontos);

        if (!await TrilhaExisteAsync(request.TrilhaId))
            return TrilhaInexistente();

        _context.Desafios.Add(desafio);
        await _context.SaveChangesAsync();

        var response = await BuscarResponseAsync(desafio.Id);

        return CreatedAtAction(nameof(ObterPorId), new { id = desafio.Id }, response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, DesafioRequest request)
    {
        var desafio = await _context.Desafios.FindAsync(id);
        if (desafio is null)
            return NaoEncontrado(id);

        if (!await TrilhaExisteAsync(request.TrilhaId))
            return TrilhaInexistente();

        desafio.Atualizar(
            request.TrilhaId,
            request.Pergunta,
            request.AlternativaA,
            request.AlternativaB,
            request.AlternativaC,
            request.AlternativaD,
            request.RespostaCorreta,
            request.Pontos);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id)
    {
        var desafio = await _context.Desafios.FindAsync(id);
        if (desafio is null)
            return NaoEncontrado(id);

        _context.Desafios.Remove(desafio);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private Task<DesafioResponse?> BuscarResponseAsync(int id) =>
        _context.Desafios
            .AsNoTracking()
            .Where(d => d.Id == id)
            .ParaResponse()
            .FirstOrDefaultAsync();

    private Task<bool> TrilhaExisteAsync(int trilhaId) =>
        _context.Trilhas.AsNoTracking().AnyAsync(t => t.Id == trilhaId);

    private NotFoundObjectResult NaoEncontrado(int id) =>
        NotFound(new ErroResponse($"Desafio {id} não encontrado."));

    private BadRequestObjectResult TrilhaInexistente() =>
        BadRequest(new ErroResponse("Trilha informada não existe."));
}
