using FnBook.Domain.Entities;
using FnBook.Domain.Interfaces;
using FnBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FnBook.Infrastructure.Repositories;

public class CommentReportRepository(AppDbContext context) : ICommentReportRepository
{
    public async Task<IEnumerable<CommentReport>> GetAllAsync() =>
        await context.CommentReports.AsNoTracking().ToListAsync();

    public async Task<CommentReport?> GetByIdAsync(Guid id) => await context.CommentReports.FindAsync(id);

    public async Task<IEnumerable<CommentReport>> GetByCommentIdAsync(Guid commentId) =>
        await context.CommentReports.AsNoTracking().Where(e => e.CommentId == commentId).ToListAsync();

    public async Task<CommentReport> CreateAsync(CommentReport entity)
    {
        context.CommentReports.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<CommentReport> UpdateAsync(CommentReport entity)
    {
        context.CommentReports.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await context.CommentReports.FindAsync(id);
        if (entity is null) return false;
        context.CommentReports.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
