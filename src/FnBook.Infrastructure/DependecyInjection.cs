using FnBook.Application.Interfaces;
using FnBook.Application.Services;
using FnBook.Domain.Interfaces;
using FnBook.Infrastructure.Data;
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

        return services;
    }
}
