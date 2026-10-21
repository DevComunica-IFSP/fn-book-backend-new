using FnBook.Domain.Entities;

namespace FnBook.Domain.Interfaces;

public interface IConsultedSourceRepository
{
    Task<IEnumerable<ConsultedSource>> GetAllAsync();
    Task<ConsultedSource?> GetByIdAsync(Guid id);
    Task<IEnumerable<ConsultedSource>> GetByNewsIdAsync(Guid newsId);
    Task<ConsultedSource> CreateAsync(ConsultedSource entity);
    Task<ConsultedSource> UpdateAsync(ConsultedSource entity);
    Task<bool> DeleteAsync(Guid id);
}
