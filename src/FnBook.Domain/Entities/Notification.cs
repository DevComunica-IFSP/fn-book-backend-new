namespace FnBook.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CommentId { get; set; }
    public string Type { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public bool IsRead { get; set; }
    public DateTime UpdatedAt { get; set; }
}
