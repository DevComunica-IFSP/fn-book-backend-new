using FnBook.Application.DTOs;
using FnBook.Domain.Entities;

namespace FnBook.Application.Mappings;

public static class NewsMapping
{
    public static NewsResponseDto ToResponseDto(this News news) => new()
    {
        Id = news.Id,
        Title = news.Title,
        Text = news.Text,
        Image = news.Image,
        CategoryId = news.CategoryId,
        ContentHash = news.ContentHash,
        CreatedBy = news.CreatedBy,
        Views = news.Views,
        AiTruth = news.AiTruth,
        Justification = news.Justification,
        AiModel = news.AiModel,
        AiConfidence = news.AiConfidence,
        AiClassificationDate = news.AiClassificationDate,
        FinalTruth = news.FinalTruth,
        IsVerified = news.IsVerified,
        VerifiedBy = news.VerifiedBy,
        VerificationDate = news.VerificationDate,
        Embedding = news.Embedding
    };

    public static News ToEntity(this CreateNewsDto dto) => new()
    {
        Id = Guid.NewGuid(),
        Title = dto.Title,
        Text = dto.Text,
        Image = dto.Image,
        CategoryId = dto.CategoryId,
        ContentHash = dto.ContentHash,
        CreatedBy = dto.CreatedBy,
        Views = 0,
        AiTruth = dto.AiTruth,
        Justification = dto.Justification,
        AiModel = dto.AiModel,
        AiConfidence = dto.AiConfidence,
        AiClassificationDate = dto.AiClassificationDate,
        FinalTruth = dto.FinalTruth,
        IsVerified = dto.IsVerified,
        VerifiedBy = dto.VerifiedBy,
        VerificationDate = dto.VerificationDate,
        Embedding = dto.Embedding
    };

    public static void ApplyUpdate(this News news, UpdateNewsDto dto)
    {
        news.Title = dto.Title;
        news.Text = dto.Text;
        news.Image = dto.Image;
        news.CategoryId = dto.CategoryId;
        news.ContentHash = dto.ContentHash;
        news.CreatedBy = dto.CreatedBy;
        news.Views = dto.Views;
        news.AiTruth = dto.AiTruth;
        news.Justification = dto.Justification;
        news.AiModel = dto.AiModel;
        news.AiConfidence = dto.AiConfidence;
        news.AiClassificationDate = dto.AiClassificationDate;
        news.FinalTruth = dto.FinalTruth;
        news.IsVerified = dto.IsVerified;
        news.VerifiedBy = dto.VerifiedBy;
        news.VerificationDate = dto.VerificationDate;
        news.Embedding = dto.Embedding;
    }
}
