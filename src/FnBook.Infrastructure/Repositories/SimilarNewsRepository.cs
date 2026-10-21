using FnBook.Domain.Entities;
using FnBook.Domain.Interfaces;
using FnBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FnBook.Infrastructure.Repositories;

public class SimilarNewsRepository(AppDbContext context) : ISimilarNewsRepository
{
    public async Task<IEnumerable<SimilarNews>> GetByNewsIdAsync(Guid newsId) =>
        await context.SimilarNews.AsNoTracking().Where(e => e.NewsId == newsId).ToListAsync();

    public async Task<SimilarNews?> GetByIdAsync(Guid newsId, Guid similarNewsId) =>
        await context.SimilarNews.FindAsync(newsId, similarNewsId);

    public async Task<SimilarNews> CreateAsync(SimilarNews entity)
    {
        context.SimilarNews.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<SimilarNews> UpdateAsync(SimilarNews entity)
    {
        context.SimilarNews.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid newsId, Guid similarNewsId)
    {
        var entity = await GetByIdAsync(newsId, similarNewsId);
        if (entity is null) return false;
        context.SimilarNews.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
