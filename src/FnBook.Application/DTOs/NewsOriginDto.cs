namespace FnBook.Application.DTOs;

public record CreateNewsOriginDto(Guid UserId, Guid NewsId, Guid UfId, DateTime Date);

public record UpdateNewsOriginDto(Guid UserId, Guid NewsId, Guid UfId, DateTime Date);

public record NewsOriginResponseDto(Guid OrigemId, Guid UserId, Guid NewsId, Guid UfId, DateTime Date);
