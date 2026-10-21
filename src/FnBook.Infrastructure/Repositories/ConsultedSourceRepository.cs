using FnBook.Domain.Entities;
using FnBook.Domain.Interfaces;
using FnBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FnBook.Infrastructure.Repositories;

public class ConsultedSourceRepository(AppDbContext context) : IConsultedSourceRepository
{
    public async Task<IEnumerable<ConsultedSource>> GetAllAsync() =>
        await context.ConsultedSources.AsNoTracking().ToListAsync();

    public async Task<ConsultedSource?> GetByIdAsync(Guid id) => await context.ConsultedSources.FindAsync(id);

    public async Task<IEnumerable<ConsultedSource>> GetByNewsIdAsync(Guid newsId) =>
        await context.ConsultedSources.AsNoTracking().Where(e => e.NewsId == newsId).ToListAsync();

    public async Task<ConsultedSource> CreateAsync(ConsultedSource entity)
    {
        context.ConsultedSources.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<ConsultedSource> UpdateAsync(ConsultedSource entity)
    {
        context.ConsultedSources.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await context.ConsultedSources.FindAsync(id);
        if (entity is null) return false;
        context.ConsultedSources.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
