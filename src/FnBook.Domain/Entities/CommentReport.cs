namespace FnBook.Domain.Entities;

public class CommentReport
{
    public Guid Id { get; set; }
    public Guid CommentId { get; set; }
    public Guid UserId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
