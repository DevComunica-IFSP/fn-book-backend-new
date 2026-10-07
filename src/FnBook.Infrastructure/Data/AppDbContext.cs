using FnBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FnBook.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<News> News => Set<News>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Usuario");

            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();

            entity.Property(u => u.Id).HasColumnName("usuario_id");
            entity.Property(u => u.Name).HasColumnName("nome_completo").HasMaxLength(200).IsRequired();
            entity.Property(u => u.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
            entity.Property(u => u.PasswordHash).HasColumnName("senha").HasMaxLength(500).IsRequired();
            entity.Property(u => u.ProfilePicture).HasColumnName("foto_perfil").HasMaxLength(500);
            entity.Property(u => u.StateId).HasColumnName("uf_id");
            entity.Property(u => u.UserType).HasColumnName("tipo_usuario").HasMaxLength(50).IsRequired();
            entity.Property(u => u.ContributorScore).HasColumnName("score_contribuidor").HasDefaultValue(0);
            entity.Property(u => u.IsActive).HasColumnName("ativo").HasDefaultValue(true);
            entity.Property(u => u.CreatedAt).HasColumnName("data_cadastro");
            entity.Property(u => u.UpdatedAt).HasColumnName("data_atualizacao");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.ToTable("Noticia");

            entity.HasKey(n => n.Id);

            entity.Property(n => n.Id).HasColumnName("noticia_id");
            entity.Property(n => n.Title).HasColumnName("titulo").HasMaxLength(255).IsRequired();
            entity.Property(n => n.Text).HasColumnName("texto").IsRequired();
            entity.Property(n => n.Image).HasColumnName("imagem").HasMaxLength(500);
            entity.Property(n => n.CategoryId).HasColumnName("categoria_id");
            entity.Property(n => n.ContentHash).HasColumnName("hash_conteudo").HasMaxLength(500);
            entity.Property(n => n.CreatedBy).HasColumnName("cadastrado_por");
            entity.Property(n => n.Views).HasColumnName("visualizacoes").HasDefaultValue(0);
            entity.Property(n => n.AiTruth).HasColumnName("verdadeiro_ia");
            entity.Property(n => n.Justification).HasColumnName("justificativa");
            entity.Property(n => n.AiModel).HasColumnName("modelo_ia").HasMaxLength(100);
            entity.Property(n => n.AiConfidence).HasColumnName("confianca_ia").HasColumnType("decimal(5,4)");
            entity.Property(n => n.AiClassificationDate).HasColumnName("data_classificacao_ia");
            entity.Property(n => n.FinalTruth).HasColumnName("verdadeiro_final");
            entity.Property(n => n.IsVerified).HasColumnName("verificado").HasDefaultValue(false);
            entity.Property(n => n.VerifiedBy).HasColumnName("verificado_por");
            entity.Property(n => n.VerificationDate).HasColumnName("data_verificacao");
            entity.Property(n => n.Embedding).HasColumnName("embedding");
        });
    }
}
