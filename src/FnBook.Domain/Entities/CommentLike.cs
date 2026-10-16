namespace FnBook.Domain.Entities;

public class CommentLike
{
    public Guid UserId { get; set; }
    public Guid CommentId { get; set; }
}
