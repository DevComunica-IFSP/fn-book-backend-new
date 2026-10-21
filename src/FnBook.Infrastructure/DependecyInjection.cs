using FnBook.Application.Interfaces;
using FnBook.Application.Services;
using FnBook.Domain.Interfaces;
using FnBook.Infrastructure.Data;
using FnBook.Infrastructure.Security;
using FnBook.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FnBook.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<INewsRepository, NewsRepository>();
        services.AddScoped<INewsService, NewsService>();
        services.AddScoped<INewsOriginRepository, NewsOriginRepository>();
        services.AddScoped<INewsOriginService, NewsOriginService>();
        services.AddScoped<IUfRepository, UfRepository>();
        services.AddScoped<IUfService, UfService>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICommentService, CommentService>();

        services.AddScoped<ICommentReportRepository, CommentReportRepository>();
        services.AddScoped<ICommentReportService, CommentReportService>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IConsultedSourceRepository, ConsultedSourceRepository>();
        services.AddScoped<IConsultedSourceService, ConsultedSourceService>();
        services.AddScoped<ISimilarNewsRepository, SimilarNewsRepository>();
        services.AddScoped<ISimilarNewsService, SimilarNewsService>();

        services.AddScoped<IPasswordHashService, PasswordHashService>();

        return services;
    }
}
