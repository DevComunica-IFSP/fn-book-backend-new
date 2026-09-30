namespace FnBook.Domain.Entities;

public class News
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string? Image { get; set; }
    public Guid? CategoryId { get; set; }
    public string? ContentHash { get; set; }
    public Guid? CreatedBy { get; set; }
    public int Views { get; set; }
    public bool? AiTruth { get; set; }
    public string? Justification { get; set; }
    public string? AiModel { get; set; }
    public decimal? AiConfidence { get; set; }
    public DateTime? AiClassificationDate { get; set; }
    public bool? FinalTruth { get; set; }
    public bool IsVerified { get; set; }
    public Guid? VerifiedBy { get; set; }
    public DateTime? VerificationDate { get; set; }
    public string? Embedding { get; set; }
}
