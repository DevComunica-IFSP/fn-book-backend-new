using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FnBook.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UfsController : ControllerBase
{
    private readonly IUfService _service;

    public UfsController(IUfService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UfResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UfResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var uf = await _service.GetByIdAsync(id);
        return uf is null ? NotFound() : Ok(uf);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UfResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateUfDto dto)
    {
        try
        {
            var uf = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = uf.UfId }, uf);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UfResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUfDto dto)
    {
        try
        {
            var uf = await _service.UpdateAsync(id, dto);
            return uf is null ? NotFound() : Ok(uf);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            return await _service.DeleteAsync(id) ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
}
