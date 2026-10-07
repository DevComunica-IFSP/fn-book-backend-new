namespace FnBook.Domain.Entities;

public class NewsOrigin
{
    public Guid OrigemId { get; set; }
    public Guid UserId { get; set; }
    public Guid NewsId { get; set; }
    public Guid UfId { get; set; }
    public DateTime Date { get; set; }
}
