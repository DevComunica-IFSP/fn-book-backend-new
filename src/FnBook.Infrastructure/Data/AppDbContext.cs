using FnBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FnBook.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<News> News => Set<News>();
    public DbSet<NewsOrigin> NewsOrigins => Set<NewsOrigin>();
    public DbSet<Uf> Ufs => Set<Uf>();

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

        modelBuilder.Entity<Uf>(entity =>
        {
            entity.ToTable("Uf");
            entity.HasKey(uf => uf.UfId);
            entity.Property(uf => uf.UfId).HasColumnName("uf_id");
            entity.Property(uf => uf.Sigla).HasColumnName("sigla").HasMaxLength(2).IsRequired();
            entity.Property(uf => uf.Nome).HasColumnName("nome").HasMaxLength(100).IsRequired();
            entity.HasIndex(uf => uf.Sigla).IsUnique();
        });

        modelBuilder.Entity<NewsOrigin>(entity =>
        {
            entity.ToTable("Origem");
            entity.HasKey(o => o.OrigemId);
            entity.Property(o => o.OrigemId).HasColumnName("origem_id");
            entity.Property(o => o.NewsId).HasColumnName("noticia_id");
            entity.Property(o => o.UserId).HasColumnName("usuario_id");
            entity.Property(o => o.UfId).HasColumnName("uf_id");
            entity.Property(o => o.Date).HasColumnName("data");
            entity.HasIndex(o => o.NewsId);
            entity.HasIndex(o => o.UserId);
            entity.HasIndex(o => o.UfId);
            entity.HasOne<News>().WithMany().HasForeignKey(o => o.NewsId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<User>().WithMany().HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Uf>().WithMany().HasForeignKey(o => o.UfId).OnDelete(DeleteBehavior.Restrict);
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

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categoria");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Id).HasColumnName("categoria_id");
            entity.Property(c => c.Name).HasColumnName("nome").HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<CommentLike>(entity =>
        {
            entity.ToTable("Curtida_Comentario");

            entity.HasKey(cl => new { cl.UserId, cl.CommentId });

            entity.Property(cl => cl.UserId).HasColumnName("usuario_id");
            entity.Property(cl => cl.CommentId).HasColumnName("comentario_id");
        });

        modelBuilder.Entity<NewsLike>(entity =>
        {
            entity.ToTable("Curtida_Noticia");

            entity.HasKey(nl => new { nl.UserId, nl.NewsId });

            entity.Property(nl => nl.UserId).HasColumnName("usuario_id");
            entity.Property(nl => nl.NewsId).HasColumnName("noticia_id");
        });
    }
}
