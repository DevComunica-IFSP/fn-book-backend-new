namespace FnBook.Application.DTOs;

public record CreateNewsDto
{
    public string Title { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
    public string? Image { get; init; }
    public Guid? CategoryId { get; init; }
    public string? ContentHash { get; init; }
    public Guid? CreatedBy { get; init; }
    public bool? AiTruth { get; init; }
    public string? Justification { get; init; }
    public string? AiModel { get; init; }
    public decimal? AiConfidence { get; init; }
    public DateTime? AiClassificationDate { get; init; }
    public bool? FinalTruth { get; init; }
    public bool IsVerified { get; init; }
    public Guid? VerifiedBy { get; init; }
    public DateTime? VerificationDate { get; init; }
    public string? Embedding { get; init; }
}

public record UpdateNewsDto
{
    public string Title { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
    public string? Image { get; init; }
    public Guid? CategoryId { get; init; }
    public string? ContentHash { get; init; }
    public Guid? CreatedBy { get; init; }
    public int Views { get; init; }
    public bool? AiTruth { get; init; }
    public string? Justification { get; init; }
    public string? AiModel { get; init; }
    public decimal? AiConfidence { get; init; }
    public DateTime? AiClassificationDate { get; init; }
    public bool? FinalTruth { get; init; }
    public bool IsVerified { get; init; }
    public Guid? VerifiedBy { get; init; }
    public DateTime? VerificationDate { get; init; }
    public string? Embedding { get; init; }
}

public record NewsResponseDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
    public string? Image { get; init; }
    public Guid? CategoryId { get; init; }
    public string? ContentHash { get; init; }
    public Guid? CreatedBy { get; init; }
    public int Views { get; init; }
    public bool? AiTruth { get; init; }
    public string? Justification { get; init; }
    public string? AiModel { get; init; }
    public decimal? AiConfidence { get; init; }
    public DateTime? AiClassificationDate { get; init; }
    public bool? FinalTruth { get; init; }
    public bool IsVerified { get; init; }
    public Guid? VerifiedBy { get; init; }
    public DateTime? VerificationDate { get; init; }
    public string? Embedding { get; init; }
}
