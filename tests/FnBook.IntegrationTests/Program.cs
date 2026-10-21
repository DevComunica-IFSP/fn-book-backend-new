using System.ComponentModel.DataAnnotations;
using FnBook.API.Controllers;
using FnBook.Application.DTOs;
using FnBook.Application.Interfaces;
using FnBook.Domain.Entities;
using FnBook.Infrastructure;
using FnBook.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Executable integration tests without additional test framework dependencies.
// Apply the complete migration history only to isolated in-memory databases.
await using var connection = new SqliteConnection("Data Source=:memory:");
await connection.OpenAsync();
var services = new ServiceCollection();
services.AddInfrastructure(new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
{
    ["ConnectionStrings:DefaultConnection"] = "Data Source=:memory:"
}).Build());
services.AddDbContext<AppDbContext>(options => options.UseSqlite(connection));
using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
using var scope = provider.CreateScope();
var sp = scope.ServiceProvider;
var db = sp.GetRequiredService<AppDbContext>();
await db.Database.MigrateAsync();
int assertions = await MigrationChecks.RunAsync();
void Check(bool condition, string message)
{
    if (!condition) throw new Exception(message);
    assertions++;
}
async Task Throws<T>(Func<Task> action) where T : Exception
{
    try { await action(); }
    catch (T) { assertions++; return; }
    throw new Exception($"Expected {typeof(T).Name}");
}
var users = sp.GetRequiredService<IUserService>();
var hasher = sp.GetRequiredService<IPasswordHashService>();
var user = await users.CreateAsync(new CreateUserDto
{
    Name = "Teste", Email = "test@example.com", Password = "senha de teste", UserType = "usuario"
});
var secondUser = await users.CreateAsync(new CreateUserDto
{
    Name = "Outro", Email = "other@example.com", Password = "senha de teste", UserType = "usuario"
});
var stored = await db.Users.AsNoTracking().SingleAsync(u => u.Id == user.Id);
var otherHash = await db.Users.AsNoTracking().Where(u => u.Id == secondUser.Id).Select(u => u.PasswordHash).SingleAsync();
Check(stored.PasswordHash != otherHash, "Equal passwords must have different salts");
Check(hasher.Verify(stored.PasswordHash, "senha de teste"), "Password verification");
Check(!hasher.Verify(stored.PasswordHash, "errada"), "Incorrect password rejected");
Check(!hasher.Verify("invalid hash", "senha de teste"), "Malformed hash rejected");
Check(!hasher.Verify(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("senha de teste")), "senha de teste"), "Legacy Base64 rejected");
await Throws<ArgumentException>(() => users.CreateAsync(new CreateUserDto { Password = " " }));
var news = new News { Id = Guid.NewGuid(), Title = "Notícia", Text = "Texto" };
var other = new News { Id = Guid.NewGuid(), Title = "Outra", Text = "Texto" };
db.News.AddRange(news, other);
await db.SaveChangesAsync();
var comments = sp.GetRequiredService<ICommentService>();
var comment = await comments.CreateAsync(new CreateCommentDto { UserId = user.Id, NewsId = news.Id, Text = "Comentário" });
var controller = new CommentsController(comments);
db.ChangeTracker.Clear();
Check((await comments.GetByIdAsync(comment.Id))?.Text == "Comentário", "New comment immediately available by ID");
Check((await comments.GetAllAsync()).Any(c => c.Id == comment.Id), "New comment immediately listed");
var edited = await comments.UpdateAsync(comment.Id, new UpdateCommentDto { UserId = user.Id, NewsId = news.Id, Text = "Editado" });
Check(edited?.Text == "Editado" && edited.Date == comment.Date, "Editing preserves creation date");
db.ChangeTracker.Clear();
Check((await comments.GetByIdAsync(comment.Id))?.Text == "Editado", "Edited comment immediately available");
Check(await controller.GetById(Guid.NewGuid()) is NotFoundResult, "Missing comment gives 404");

var reports = sp.GetRequiredService<ICommentReportService>();
var reportDto = new CreateCommentReportDto { UserId = user.Id, CommentId = comment.Id, Reason = "Ofensa" };
var reportController = new CommentReportsController(reports);
Check(await reportController.Create(reportDto with { CommentId = Guid.NewGuid() }) is BadRequestObjectResult, "Report references validated as 400");
await Throws<ValidationException>(() => reports.CreateAsync(reportDto with { Reason = " " }));
var report = await reports.CreateAsync(reportDto);
Check((await reports.GetByCommentIdAsync(comment.Id)).Single().Id == report.Id, "Reports by comment");
var updatedReport = await reports.UpdateAsync(report.Id, new UpdateCommentReportDto { UserId = user.Id, CommentId = comment.Id, Reason = "Spam" });
Check(updatedReport?.Reason == "Spam" && updatedReport.Date == report.Date, "Report update preserves date");
Check(await reports.UpdateAsync(Guid.NewGuid(), new UpdateCommentReportDto()) is null, "Missing report update");

var notifications = sp.GetRequiredService<INotificationService>();
var notificationDto = new CreateNotificationDto { UserId = user.Id, CommentId = comment.Id, Type = "resposta", Quantity = 2 };
await Throws<ArgumentException>(() => notifications.CreateAsync(notificationDto with { UserId = Guid.NewGuid() }));
await Throws<ValidationException>(() => notifications.CreateAsync(notificationDto with { Quantity = 0 }));
var notification = await notifications.CreateAsync(notificationDto);
Check(!notification.IsRead && (await notifications.GetByUserIdAsync(user.Id)).Count() == 1, "Unread notification by user");
Check(!(await notifications.GetByUserIdAsync(secondUser.Id)).Any(), "Notification user filter");
Check((await notifications.MarkAsReadAsync(notification.Id))?.IsRead == true, "Mark notification read");
var updatedNotification = await notifications.UpdateAsync(notification.Id, new UpdateNotificationDto { UserId = user.Id, CommentId = comment.Id, Type = "curtida", Quantity = 3 });
Check(updatedNotification?.Quantity == 3 && updatedNotification.Type == "curtida", "Notification update");
Check(await new NotificationsController(notifications).MarkAsRead(Guid.NewGuid()) is NotFoundResult, "Missing notification gives 404");

var sources = sp.GetRequiredService<IConsultedSourceService>();
var sourceDto = new CreateConsultedSourceDto { NewsId = news.Id, Url = "https://example.com/noticia", Summary = "Resumo" };
await Throws<ArgumentException>(() => sources.CreateAsync(sourceDto with { NewsId = Guid.NewGuid() }));
await Throws<ArgumentException>(() => sources.CreateAsync(sourceDto with { Url = "ftp://example.com/file" }));
await Throws<ValidationException>(() => sources.CreateAsync(sourceDto with { Url = "not a URL" }));
var source = await sources.CreateAsync(sourceDto);
Check((await sources.GetByNewsIdAsync(news.Id)).Single().Id == source.Id, "Sources by news");
var updatedSource = await sources.UpdateAsync(source.Id, new UpdateConsultedSourceDto { NewsId = other.Id, Url = "https://example.com/outra", Summary = "Atualizado" });
Check(updatedSource?.Summary == "Atualizado" && !(await sources.GetByNewsIdAsync(news.Id)).Any(), "Source update and news filter");

var similar = sp.GetRequiredService<ISimilarNewsService>();
var similarDto = new CreateSimilarNewsDto { NewsId = news.Id, SimilarNewsId = other.Id, Similarity = 0.85m };
var similarController = new SimilarNewsController(similar);
Check(await similarController.Create(similarDto with { SimilarNewsId = news.Id }) is BadRequestObjectResult, "Self relation gives 400");
await Throws<ArgumentException>(() => similar.CreateAsync(similarDto with { SimilarNewsId = Guid.NewGuid() }));
await Throws<ValidationException>(() => similar.CreateAsync(similarDto with { Similarity = 1.01m }));
await Throws<ValidationException>(() => similar.CreateAsync(similarDto with { Similarity = -0.1m }));
Check(await similarController.Create(similarDto) is CreatedAtActionResult, "Similar news creation gives 201");
Check(await similarController.Create(similarDto) is ConflictObjectResult, "Duplicate pair gives 409");
Check((await similar.GetByNewsIdAsync(news.Id)).Single().Similarity == 0.85m, "Similarity persisted");
Check((await similar.UpdateAsync(news.Id, other.Id, new UpdateSimilarNewsDto { Similarity = 0.5m }))?.Similarity == 0.5m, "Similarity update");
Check(await similarController.GetById(Guid.NewGuid(), other.Id) is NotFoundResult, "Missing pair gives 404");

db.ChangeTracker.Clear();
Check((await notifications.GetByIdAsync(notification.Id))?.Quantity == 3, "Notification persisted");
Check((await reports.GetByIdAsync(report.Id))?.Reason == "Spam", "Report persisted");
Check((await sources.GetByIdAsync(source.Id))?.NewsId == other.Id, "Source persisted");
Check(await reports.DeleteAsync(report.Id) && !await reports.DeleteAsync(report.Id), "Report deletion and missing record");
Check(await notifications.DeleteAsync(notification.Id) && !await notifications.DeleteAsync(notification.Id), "Notification deletion and missing record");
Check(await sources.DeleteAsync(source.Id) && !await sources.DeleteAsync(source.Id), "Source deletion and missing record");
Check(await similar.DeleteAsync(news.Id, other.Id) && !await similar.DeleteAsync(news.Id, other.Id), "Composite key deletion and missing record");
// Removing a published comment also removes dependent reports and notifications.
await reports.CreateAsync(reportDto);
await notifications.CreateAsync(notificationDto);
Check(await controller.Delete(comment.Id) is NoContentResult, "Comment deletion gives 204");
db.ChangeTracker.Clear();
Check(await controller.GetById(comment.Id) is NotFoundResult, "Deleted comment unavailable");
Check(!(await comments.GetAllAsync()).Any(c => c.Id == comment.Id), "Deleted comment no longer listed");
Check(!(await reports.GetByCommentIdAsync(comment.Id)).Any(), "Comment reports deleted in cascade");
Check(!(await notifications.GetByUserIdAsync(user.Id)).Any(), "Comment notifications deleted in cascade");
Check(await controller.Delete(comment.Id) is NotFoundResult, "Deleting missing comment gives 404");
// Verify that both news FKs are enforced by SQLite, independently of service validation.
db.SimilarNews.Add(new SimilarNews { NewsId = news.Id, SimilarNewsId = Guid.NewGuid(), Similarity = 0.5m, Date = DateTime.UtcNow });
await Throws<DbUpdateException>(() => db.SaveChangesAsync());
db.ChangeTracker.Clear();
Console.WriteLine($"Passed {assertions} integration assertions (SQLite, services, controllers and password hashing).");
