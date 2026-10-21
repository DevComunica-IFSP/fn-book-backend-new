using FnBook.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

internal static class MigrationChecks
{
    public static async Task<int> RunAsync()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var db = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options);
        var migrator = db.GetService<IMigrator>();
        const string previousMigration = "20261019005532_SyncCurrentModel";
        await migrator.MigrateAsync(previousMigration);
        var userId = Guid.NewGuid();
        var newsId = Guid.NewGuid();
        var ufId = Guid.NewGuid();
        var originId = Guid.NewGuid();
        var commentId = Guid.NewGuid();
        var date = DateTime.UtcNow;
        await db.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO "Users" ("Id", "Name", "Email", "PasswordHash", "IsActive", "CreatedAt")
            VALUES ({userId}, 'Usuário existente', 'legacy@example.com', 'hash-existente', 1, {date});
            """);
        await db.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO "Noticia" ("noticia_id", "titulo", "texto") VALUES ({newsId}, 'Notícia existente', 'Texto');
            """);
        await db.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO "Uf" ("uf_id", "sigla", "nome") VALUES ({ufId}, 'SP', 'São Paulo');
            """);
        await db.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO "Origem" ("origem_id", "usuario_id", "noticia_id", "uf_id", "data")
            VALUES ({originId}, {userId}, {newsId}, {ufId}, {date});
            """);
        await db.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO "Comentario" ("comentario_id", "usuario_id", "noticia_id", "texto", "data", "status", "moderado_por", "data_moderacao")
            VALUES ({commentId}, {userId}, {newsId}, 'Comentário existente', {date}, 'aprovado', {userId}, {date});
            """);

        int assertions = 0;
        void Check(bool condition, string message)
        {
            if (!condition) throw new Exception(message);
            assertions++;
        }
        async Task<long> Scalar(string sql)
        {
            await using var command = connection.CreateCommand();
            command.CommandText = sql;
            return Convert.ToInt64(await command.ExecuteScalarAsync());
        }

        await db.Database.MigrateAsync();
        var user = await db.Users.AsNoTracking().SingleAsync();
        Check(user.Id == userId && user.Name == "Usuário existente" && user.Email == "legacy@example.com" &&
            user.PasswordHash == "hash-existente" && user.IsActive && user.CreatedAt == date,
            "Migrating legacy Users must preserve account data");
        Check(user.ContributorScore == 0 && user.UserType == "" && user.StateId is null && user.ProfilePicture is null,
            "New account fields initialized");
        Check((await db.NewsOrigins.AsNoTracking().SingleAsync()).UserId == userId, "Existing origins preserved");
        Check((await db.Comments.AsNoTracking().SingleAsync()).Text == "Comentário existente", "Existing comments preserved");
        Check(await Scalar("SELECT COUNT(*) FROM pragma_table_info('Comentario') WHERE name IN ('status', 'moderado_por', 'data_moderacao')") == 0,
            "Obsolete moderation columns removed");
        Check(await Scalar("SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name IN ('Categoria', 'Curtida_Comentario', 'Curtida_Noticia', 'Denuncia_Comentario', 'Notificacao', 'Fonte_Consultada', 'Noticia_Similar')") == 7,
            "All pending tables created");
        Check(await Scalar("SELECT COUNT(*) FROM pragma_foreign_key_check") == 0, "Migrated foreign keys valid");
        Check(await Scalar("SELECT COUNT(*) FROM pragma_foreign_key_list('Origem') WHERE \"table\" = 'Usuario' AND \"to\" = 'usuario_id'") == 1,
            "Origins reference migrated user table");
        Check(!db.Database.HasPendingModelChanges() && !(await db.Database.GetPendingMigrationsAsync()).Any(),
            "Snapshot and migration history synchronized");

        await migrator.MigrateAsync(previousMigration);
        Check(await Scalar("SELECT COUNT(*) FROM Users WHERE Name = 'Usuário existente' AND PasswordHash = 'hash-existente'") == 1,
            "Rollback preserves original account fields");
        Check(await Scalar("SELECT COUNT(*) FROM Origem") == 1 && await Scalar("SELECT COUNT(*) FROM Comentario") == 1,
            "Rollback preserves origins and comments");
        Check(await Scalar("SELECT COUNT(*) FROM pragma_table_info('Comentario') WHERE name IN ('status', 'moderado_por', 'data_moderacao')") == 3,
            "Rollback restores previous comment schema");
        Check(await Scalar("SELECT COUNT(*) FROM pragma_foreign_key_check") == 0, "Rollback foreign keys valid");
        await db.Database.MigrateAsync();
        Check((await db.Users.AsNoTracking().SingleAsync()).Id == userId && await db.NewsOrigins.CountAsync() == 1,
            "Migrations can be reapplied after rollback");
        return assertions;
    }
}
