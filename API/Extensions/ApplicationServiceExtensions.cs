using API.Helpers;
using API.SignalR;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<PresenceTracker>();

        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPhotoService, PhotoService>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<LogUserActivity>();

        services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

        services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

        services.AddDbContext<DataContext>(options => 
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}
