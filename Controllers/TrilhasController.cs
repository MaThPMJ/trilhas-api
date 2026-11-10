using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhasApi.Data;
using TrilhasApi.DTOs;
using TrilhasApi.Models;

namespace TrilhasApi.Controllers;

[ApiController]
[Route("api/trilhas")]
[Produces("application/json")]
public class TrilhasController : ControllerBase
{
    private readonly AppDbContext _context;

    public TrilhasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TrilhaResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TrilhaResponse>>> Listar()
    {
        var trilhas = await _context.Trilhas
            .AsNoTracking()
            .OrderBy(t => t.Id)
            .ParaResponse()
            .ToListAsync();

        return Ok(trilhas);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TrilhaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TrilhaResponse>> ObterPorId(int id)
    {
        var trilha = await BuscarResponseAsync(id);

        return trilha is null ? NaoEncontrada(id) : Ok(trilha);
    }

    [HttpGet("{id:int}/desafios")]
    [ProducesResponseType(typeof(IEnumerable<DesafioResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<DesafioResponse>>> ListarDesafios(int id)
    {
        var trilhaExiste = await _context.Trilhas.AsNoTracking().AnyAsync(t => t.Id == id);
        if (!trilhaExiste)
            return NaoEncontrada(id);

        var desafios = await _context.Desafios
            .AsNoTracking()
            .Where(d => d.TrilhaId == id)
            .OrderBy(d => d.Id)
            .ParaResponse()
            .ToListAsync();

        return Ok(desafios);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TrilhaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TrilhaResponse>> Criar(TrilhaRequest request)
    {
        var trilha = new Trilha(request.Titulo, request.Descricao, request.Tema!.Value, request.Nivel);

        _context.Trilhas.Add(trilha);
        await _context.SaveChangesAsync();

        var response = await BuscarResponseAsync(trilha.Id);

        return CreatedAtAction(nameof(ObterPorId), new { id = trilha.Id }, response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, TrilhaRequest request)
    {
        var trilha = await _context.Trilhas.FindAsync(id);
        if (trilha is null)
            return NaoEncontrada(id);

        trilha.Atualizar(request.Titulo, request.Descricao, request.Tema!.Value, request.Nivel);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErroResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id)
    {
        var trilha = await _context.Trilhas.FindAsync(id);
        if (trilha is null)
            return NaoEncontrada(id);

        // Os desafios da trilha são removidos pelo ON DELETE CASCADE da FK.
        _context.Trilhas.Remove(trilha);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private Task<TrilhaResponse?> BuscarResponseAsync(int id) =>
        _context.Trilhas
            .AsNoTracking()
            .Where(t => t.Id == id)
            .ParaResponse()
            .FirstOrDefaultAsync();

    private NotFoundObjectResult NaoEncontrada(int id) =>
        NotFound(new ErroResponse($"Trilha {id} não encontrada."));
}
