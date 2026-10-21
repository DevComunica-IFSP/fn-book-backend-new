using System.ComponentModel.DataAnnotations;
using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FnBook.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultedSourcesController(IConsultedSourceService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ConsultedSourceResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll() => Ok(await service.GetAllAsync());

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ConsultedSourceResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var entity = await service.GetByIdAsync(id);
        return entity is null ? NotFound() : Ok(entity);
    }

    [HttpGet("by-news/{newsId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<ConsultedSourceResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByNewsId(Guid newsId) =>
        Ok(await service.GetByNewsIdAsync(newsId));

    [HttpPost]
    [ProducesResponseType(typeof(ConsultedSourceResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateConsultedSourceDto dto)
    {
        try
        {
            var entity = await service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }
        catch (Exception ex) when (ex is ArgumentException or ValidationException)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ConsultedSourceResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateConsultedSourceDto dto)
    {
        try
        {
            var entity = await service.UpdateAsync(id, dto);
            return entity is null ? NotFound() : Ok(entity);
        }
        catch (Exception ex) when (ex is ArgumentException or ValidationException)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id) =>
        await service.DeleteAsync(id) ? NoContent() : NotFound();
}
