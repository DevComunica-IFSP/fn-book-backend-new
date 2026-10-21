namespace FnBook.Domain.Entities;

public class SimilarNews
{
    public Guid NewsId { get; set; }
    public Guid SimilarNewsId { get; set; }
    public decimal Similarity { get; set; }
    public DateTime Date { get; set; }
}
