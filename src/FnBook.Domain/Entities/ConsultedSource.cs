namespace FnBook.Domain.Entities;

public class ConsultedSource
{
    public Guid Id { get; set; }
    public Guid NewsId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
}
